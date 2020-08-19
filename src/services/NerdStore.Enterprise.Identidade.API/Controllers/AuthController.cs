using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Jwks.Manager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using NerdStore.Enterprise.MessageBus;
using NerdStore.Enterprise.WebAPI.Core.Usuario;
using NerdStore.Enterprise.Identidade.API.Data;
using NerdStore.Enterprise.Identidade.API.Models;
using NerdStore.Enterprise.WebAPI.Core.Controllers;
using NerdStore.Enterprise.Core.Messages.Integration;
using NerdStore.Enterprise.Identidade.API.Extensions;

namespace NerdStore.Enterprise.Identidade.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        public AuthController(
            IMessageBus bus,
            IAspNetUser user,
            ApplicationDbContext context,
            IJsonWebKeySetService jwksService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOptions<AppTokenSettings> appTokenSettings)
        {
            _bus = bus;
            _user = user;
            _context = context;
            _jwksService = jwksService;
            _userManager = userManager;
            _signInManager = signInManager;
            _appTokenSettings = appTokenSettings.Value;
        }

        private readonly IMessageBus _bus;
        private readonly IAspNetUser _user;
        private readonly ApplicationDbContext _context;
        private readonly IJsonWebKeySetService _jwksService;
        private readonly AppTokenSettings _appTokenSettings;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
                
        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistroViewModel parametros)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var user = new IdentityUser
            {
                UserName = parametros.Email,
                Email = parametros.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, parametros.Senha);
            if (result.Succeeded)
            {
                var clienteResult = await RegistrarCliente(parametros);
                if (!clienteResult.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(clienteResult.ValidationResult);
                }

                return CustomResponse(await GerarJwt(user.Email));
            }

            foreach (var erro in result.Errors)
                AdicionarErroProcessamento(erro.Description);

            return CustomResponse();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLoginViewModel parametros)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(parametros.Email, parametros.Senha, false, true);
            if (result.Succeeded) return CustomResponse(await GerarJwt(parametros.Email));

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou senha inválidos");
            return CustomResponse();
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                AdicionarErroProcessamento("Refresh Token inválido");
                return CustomResponse();
            }

            var token = await ObterRefreshToken(Guid.Parse(refreshToken));
            if (token is null)
            {
                AdicionarErroProcessamento("Refresh Token expirado");
                return CustomResponse();
            }

            return CustomResponse(GerarJwt(token.UserName));
        }

        private async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
                claims.Add(new Claim("role", userRole));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _jwksService.GetCurrent();

            var currentIssuer = $"{_user.ObterHttpContext().Request.Scheme}://{_user.ObterHttpContext().Request.Host}";

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = key
            });

            var encodedToken = tokenHandler.WriteToken(token);
            var refreshToken = await GerarRefreshToken(email);

            var response = new UsuarioRespostaLogin
            {
                AccessToken = encodedToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime dateTime)
        {
            return (long)Math.Round((dateTime.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistroViewModel usuarioRegistro)
        {
            var usuario = await _userManager.FindByEmailAsync(usuarioRegistro.Email);

            var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(
                Guid.Parse(usuario.Id), 
                usuarioRegistro.Nome, 
                usuarioRegistro.Email, 
                usuarioRegistro.Cpf);

            try { return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado); }
            catch { await _userManager.DeleteAsync(usuario); throw; }
        }

        private async Task<RefreshToken> GerarRefreshToken(string email)
        {
            var refreshToken = new RefreshToken
            {
                UserName = email,
                ExpirationDate = DateTime.UtcNow.AddHours(_appTokenSettings.RefreshTokenExpiration)
            };

            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(x => x.UserName == email));
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private async Task<RefreshToken> ObterRefreshToken(Guid refreshToken)
        {
            var token = await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(u => u.Token == refreshToken);
            return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now ? token : null;
        }
    }
}
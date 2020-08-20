using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.Core.Communication;
using NerdStore.Enterprise.WebAPI.Core.Usuario;
using NerdStore.Enterprise.WebApp.MVC.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        public AutenticacaoService(
            HttpClient httpClient,
            IAspNetUser aspNetUser,
            IOptions<AppSettings> options,
            IAuthenticationService authenticationService)
        {
            _aspNetUser = aspNetUser;
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            _httpClient.BaseAddress = new Uri(options.Value.AutenticacaoUrl);
        }

        
        private readonly HttpClient _httpClient;
        private readonly IAspNetUser _aspNetUser;
        private readonly IAuthenticationService _authenticationService;

        public async Task<UsuarioRespostaLogin> Login(UsuarioLoginViewModel parametros)
        {
            var response = await _httpClient.PostAsync("/api/identidade/autenticar", ObterConteudo(parametros));

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistroViewModel parametros)
        {
            var response = await _httpClient.PostAsync("/api/identidade/nova-conta", ObterConteudo(parametros));

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task RealizarLogin(UsuarioRespostaLogin usuario)
        {
            var token = ObterJwtToken(usuario.AccessToken);
            var claims = new List<Claim>
            {
                new Claim("JWT", usuario.AccessToken),
                new Claim("RefreshToken", usuario.RefreshToken)
            };
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true
            };

            await _authenticationService.SignInAsync(_aspNetUser.ObterHttpContext(), CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(_aspNetUser.ObterHttpContext(), CookieAuthenticationDefaults.AuthenticationScheme, null);
        }

        public JwtSecurityToken ObterJwtToken(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

        public bool TokenExpirado()
        {
            var jwt = _aspNetUser.ObterUserToken();
            if (jwt is null) return false;

            var token = ObterJwtToken(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken)
        {
            var refreshTokenContent = ObterConteudo(refreshToken);
            var response = await _httpClient.PostAsync("/api/identidade/refresh-token", refreshTokenContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task<bool> RefreshTokenValido()
        {
            var resposta = await UtilizarRefreshToken(_aspNetUser.ObterUserRefreshToken());
            if (resposta.AccessToken != null && resposta.ResponseResult == null)
            {
                await RealizarLogin(resposta);
                return true;
            }

            return false;
        }
    }
}
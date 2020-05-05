using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NerdStore.Enterprise.Identidade.API.Models;

namespace NerdStore.Enterprise.Identidade.API.Controllers
{
    [Route("api/identidade")]
    public class AuthController : Controller
    {
        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistroViewModel parametros)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = new IdentityUser
            {
                UserName = parametros.Email,
                Email = parametros.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, parametros.Senha);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLoginViewModel parametros)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _signInManager.PasswordSignInAsync(parametros.Email, parametros.Senha, false, true);
            
            if (result.Succeeded) return Ok();
            return BadRequest();
        }
    }
}
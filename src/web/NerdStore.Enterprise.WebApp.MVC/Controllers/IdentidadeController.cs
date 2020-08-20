using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NerdStore.Enterprise.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {
        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        private readonly IAutenticacaoService _autenticacaoService;

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistroViewModel parametros)
        {
            if (!ModelState.IsValid) return View(parametros);

            var resposta = await _autenticacaoService.Registro(parametros);
            if (ResponsePossuiErros(resposta.ResponseResult)) return View(parametros);

            await _autenticacaoService.RealizarLogin(resposta);
            return RedirectToAction("Index", "Catalogo");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLoginViewModel parametros, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(parametros);

            var resposta = await _autenticacaoService.Login(parametros);
            if (ResponsePossuiErros(resposta.ResponseResult)) return View(parametros);

            await _autenticacaoService.RealizarLogin(resposta);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalogo");
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Identidade");
        }
    }
}
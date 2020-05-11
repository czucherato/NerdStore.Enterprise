using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.WebApp.MVC.Services;

namespace NerdStore.Enterprise.WebApp.MVC.Controllers
{
    public class IdentidadeController : Controller
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
            //API - Registro
            var resposta = await _autenticacaoService.Registro(parametros);
            if (false) return View(parametros);

            //Realizar login na APP
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UsuarioLoginViewModel parametros)
        {
            if (!ModelState.IsValid) return View(parametros);
            //API - Login
            var resposta = await _autenticacaoService.Login(parametros);
            if (false) return View(parametros);

            //Realizar login na APP
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> LogOut()
        {
            return RedirectToAction("Login", "Identidade");
        }
    }
}
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

            await RealizarLogin(resposta);
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

            await RealizarLogin(resposta);

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

        private async Task RealizarLogin(UsuarioRespostaLogin usuario)
        {
            var token = ObterJwtToken(usuario.AccessToken);
            var claims = new List<Claim>
            {
                new Claim("JWT", usuario.AccessToken)
            };
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        private static JwtSecurityToken ObterJwtToken(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }
    }
}
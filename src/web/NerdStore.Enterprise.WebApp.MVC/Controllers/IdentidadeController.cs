﻿using System;
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
            await RealizarLogin(resposta);
            //if (false) return View(parametros);

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
            await RealizarLogin(resposta);
            //if (false) return View(parametros);

            //Realizar login na APP
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> LogOut()
        {
            return RedirectToAction("Login", "Identidade");
        }

        private async Task RealizarLogin(UsuarioRespostaLogin usuario)
        {
            var token = ObterJwtToken(usuario.AccessToken);
            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", usuario.AccessToken));
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
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Enterprise.WebApp.MVC.Services;

namespace NerdStore.Enterprise.WebApp.MVC.Controllers
{
    public class CatalogoController : MainController
    {
        public CatalogoController(
            ICatalogoService catalogoService,
            ICatalogoServiceRefit catalogoServiceRefit)
        {
            _catalogoService = catalogoService;
            _catalogoServiceRefit = catalogoServiceRefit;
        }

        private readonly ICatalogoService _catalogoService;
        private readonly ICatalogoServiceRefit _catalogoServiceRefit;

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            return View(await _catalogoServiceRefit.ObterTodos());
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            return View(await _catalogoServiceRefit.ObterPorId(id));
        }
    }
}
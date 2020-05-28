using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Enterprise.WebApp.MVC.Services;

namespace NerdStore.Enterprise.WebApp.MVC.Controllers
{
    public class CatalogoController : MainController
    {
        public CatalogoController(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        private readonly ICatalogoService _catalogoService; 

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            return View(await _catalogoService.ObterTodos());
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            return View(await _catalogoService.ObterPorId(id));
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using NerdStore.Enterprise.Catalogo.API.Models;
using NerdStore.Enterprise.WebAPI.Core.Identidade;
using NerdStore.Enterprise.WebAPI.Core.Controllers;

namespace NerdStore.Enterprise.Catalogo.API.Controllers
{
    [Authorize]
    public class CatalogoController : MainController
    {
        public CatalogoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        private readonly IProdutoRepository _produtoRepository;

        [AllowAnonymous]
        [HttpGet("catalogo/produtos")]
        public async Task<IEnumerable<Produto>> Index()
        {
            return await _produtoRepository.ObterTodos();
        }

        [HttpGet("catalogo/produtos/{id}")]
        [ClaimsAuthorize("Catalogo", "Ler")]
        public async Task<Produto> ProdutoDetalhe(Guid id)
        {
            return await _produtoRepository.ObterPorId(id);
        }

        [HttpGet("catalogo/produtos/lista/{ids}")]
        public async Task<IEnumerable<Produto>> ObterProdutosPorId(string ids)
        {
            return await _produtoRepository.ObterProdutosPorId(ids);
        }
    }
}
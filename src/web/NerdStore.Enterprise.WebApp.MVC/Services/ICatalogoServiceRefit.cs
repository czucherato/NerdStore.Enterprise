using Refit;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NerdStore.Enterprise.WebApp.MVC.Models;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public interface ICatalogoServiceRefit
    {
        [Get("/catalogo/produtos/")]
        Task<IEnumerable<ProdutoViewModel>> ObterTodos();

        [Get("/catalogo/produtos/{id}")]
        Task<ProdutoViewModel> ObterPorId(Guid id);
    }
}
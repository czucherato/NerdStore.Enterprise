using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NerdStore.Enterprise.WebApp.MVC.Models;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public interface ICatalogoService
    {
        Task<IEnumerable<ProdutoViewModel>> ObterTodos();

        Task<ProdutoViewModel> ObterPorId(Guid id);
    }
}
using System;
using System.Threading.Tasks;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.Core.Communication;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public interface ICarrinhoService
    {
        Task<CarrinhoViewModel> ObterCarrinho();

        Task<ResponseResult> AdicionarItemCarrinho(ItemProdutoViewModel produto);

        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto);

        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    }
}
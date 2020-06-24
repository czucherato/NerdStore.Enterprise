using System;
using System.Threading.Tasks;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.Core.Communication;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public interface IComprasBffService
    {
        Task<CarrinhoViewModel> ObterCarrinho();

        Task<int> ObterQuantidadeCarrinho();

        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel produto);

        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto);

        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);


        Task<ResponseResult> AplicarVoucherCarrinho(string voucher);
    }
}
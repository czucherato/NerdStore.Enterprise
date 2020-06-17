using System;
using System.Threading.Tasks;
using NerdStore.Enterprise.BFF.Compras.Models;
using NerdStore.Enterprise.Core.Communication;

namespace NerdStore.Enterprise.BFF.Compras.Services
{
    public interface ICarrinhoService
    {
        Task<CarrinhoDTO> ObterCarrinho();

        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO produto);

        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO produto);

        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    }
}
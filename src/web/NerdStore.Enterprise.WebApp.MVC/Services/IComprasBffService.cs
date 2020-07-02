using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.Core.Communication;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public interface IComprasBffService
    {
        // Carrinho
        Task<CarrinhoViewModel> ObterCarrinho();

        Task<int> ObterQuantidadeCarrinho();

        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho);

        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel carrinho);

        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);

        Task<ResponseResult> AplicarVoucherCarrinho(string voucher);


        // Pedido
        Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);

        Task<PedidoViewModel> ObterUltimoPedido();

        Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId();

        PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco);
    }
}
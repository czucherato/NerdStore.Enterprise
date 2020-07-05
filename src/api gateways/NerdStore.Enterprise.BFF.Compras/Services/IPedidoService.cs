using System.Threading.Tasks;
using System.Collections.Generic;
using NerdStore.Enterprise.BFF.Compras.Models;
using NerdStore.Enterprise.Core.Communication;

namespace NerdStore.Enterprise.BFF.Compras.Services
{
    public interface IPedidoService
    {
        Task<ResponseResult> FinalizarPedido(PedidoDTO pedido);

        Task<PedidoDTO> ObterUltimoPedido();

        Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId();

        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }
}
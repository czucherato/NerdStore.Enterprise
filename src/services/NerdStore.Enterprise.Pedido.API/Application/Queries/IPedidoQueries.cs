using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NerdStore.Enterprise.Pedido.API.Application.DTO;

namespace NerdStore.Enterprise.Pedido.API.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<PedidoDTO> ObterUltimoPedido(Guid clientId);

        Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId(Guid clienteId);
    }
}
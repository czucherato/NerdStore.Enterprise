using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NerdStore.Enterprise.Core.Data;
using System.Data.Common;

namespace NerdStore.Enterprise.Pedido.Domain.Pedidos
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        DbConnection ObterConexao();

        Task<Pedido> ObterPorId(Guid id);

        Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId);

        void Adicionar(Pedido pedido);

        void Atualizar(Pedido pedido);

        Task<PedidoItem> ObterItemPorId(Guid id);

        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produdoId);
    }
}
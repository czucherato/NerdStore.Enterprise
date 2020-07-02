using System;
using System.Linq;
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NerdStore.Enterprise.Core.Data;
using NerdStore.Enterprise.Pedido.Domain.Pedidos;

namespace NerdStore.Enterprise.Pedido.Infra.Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        public PedidoRepository(PedidosContext context)
        {
            _context = context;
        }

        private readonly PedidosContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public DbConnection ObterConexao() => _context.Database.GetDbConnection();

        public async Task<Domain.Pedidos.Pedido> ObterPorId(Guid id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task<IEnumerable<Domain.Pedidos.Pedido>> ObterListaPorClienteId(Guid clienteId)
        {
            return await _context.Pedidos
                .Include(p => p.PedidoItems)
                .AsNoTracking().Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

        public void Adicionar(Domain.Pedidos.Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public void Atualizar(Domain.Pedidos.Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }

        public async Task<PedidoItem> ObterItemPorId(Guid id)
        {
            return await _context.PedidoItems.FindAsync(id);
        }

        public async Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produdoId)
        {
            return await _context.PedidoItems
                .FirstOrDefaultAsync(p => p.ProdutoId == produdoId && p.PedidoId == pedidoId);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
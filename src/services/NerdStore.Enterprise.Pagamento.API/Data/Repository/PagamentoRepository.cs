using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NerdStore.Enterprise.Core.Data;
using NerdStore.Enterprise.Pagamento.API.Models;

namespace NerdStore.Enterprise.Pagamento.API.Data.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        public PagamentoRepository(PagamentoContext context)
        {
            _context = context;
        }

        private readonly PagamentoContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public void AdicionarPagamento(Models.Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
        }

        public void AdicionarTransacao(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
        }

        public async Task<IEnumerable<Transacao>> ObterTransacoesPorPedidoId(Guid pedidoId)
        {
            return await _context.Transacoes.AsNoTracking().Where(t => t.Pagamento.PedidoId == pedidoId).ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
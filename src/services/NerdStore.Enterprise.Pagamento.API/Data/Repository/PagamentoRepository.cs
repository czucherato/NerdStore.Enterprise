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

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
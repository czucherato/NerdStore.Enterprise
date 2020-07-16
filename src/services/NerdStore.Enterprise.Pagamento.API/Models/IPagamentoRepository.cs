using NerdStore.Enterprise.Core.Data;

namespace NerdStore.Enterprise.Pagamento.API.Models
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        void AdicionarPagamento(Pagamento pagamento);
    }
}
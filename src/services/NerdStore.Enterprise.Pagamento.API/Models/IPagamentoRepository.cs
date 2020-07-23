using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using NerdStore.Enterprise.Core.Data;

namespace NerdStore.Enterprise.Pagamento.API.Models
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        void AdicionarPagamento(Pagamento pagamento);

        void AdicionarTransacao(Transacao transacao);

        Task<IEnumerable<Transacao>> ObterTransacoesPorPedidoId(Guid pedidoId);
    }
}
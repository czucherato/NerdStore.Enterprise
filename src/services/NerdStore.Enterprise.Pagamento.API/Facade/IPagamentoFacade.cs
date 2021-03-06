﻿using System.Threading.Tasks;
using NerdStore.Enterprise.Pagamento.API.Models;

namespace NerdStore.Enterprise.Pagamento.API.Facade
{
    public interface IPagamentoFacade
    {
        Task<Transacao> AutorizarPagamento(Models.Pagamento pagamento);

        Task<Transacao> CapturarPagamento(Transacao transacao);

        Task<Transacao> CancelarPagamento(Transacao transacao);
    }
}
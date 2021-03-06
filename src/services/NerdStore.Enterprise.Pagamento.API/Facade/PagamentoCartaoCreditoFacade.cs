﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NerdStore.Enterprise.Pagamento.NerdsPag;
using NerdStore.Enterprise.Pagamento.API.Models;

namespace NerdStore.Enterprise.Pagamento.API.Facade
{
    public class PagamentoCartaoCreditoFacade : IPagamentoFacade
    {
        private readonly PagamentoConfig _config;

        public PagamentoCartaoCreditoFacade(IOptions<PagamentoConfig> config)
        {
            _config = config.Value;
        }

        public async Task<Transacao> AutorizarPagamento(Models.Pagamento pagamento)
        {
            var nerdsPagSvc = new NerdsPagService(_config.DefaultApiKey, _config.DefaultEncryptionKey);
            var cardHashGen = new CardHash(nerdsPagSvc)
            {
                CardNumber = pagamento.CartaoCredito.NumeroCartao,
                CardHolderName = pagamento.CartaoCredito.NomeCartao,
                CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
                CardCvv = pagamento.CartaoCredito.CVV
            };

            var cardHash = cardHashGen.Generate();
            var transacao = new Transaction(nerdsPagSvc)
            {
                CardHash = cardHash,
                CardNumber = pagamento.CartaoCredito.NumeroCartao,
                CardHolderName = pagamento.CartaoCredito.NomeCartao,
                CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
                CardCvv = pagamento.CartaoCredito.CVV,
                PaymentMethod = PaymentMethod.CreditCard,
                Amount = pagamento.Valor
            };

            return ParaTransacao(await transacao.AuthorizeCardTransaction());
        }

        public async Task<Transacao> CapturarPagamento(Transacao transacao)
        {
            var nerdsPagSvc = new NerdsPagService(_config.DefaultApiKey, _config.DefaultEncryptionKey);
            var transaction = ParaTransaction(transacao, nerdsPagSvc);
            return ParaTransacao(await transaction.CaptureCardTransaction());
        }

        public async Task<Transacao> CancelarPagamento(Transacao transacao)
        {
            var nerdsPagSvc = new NerdsPagService(_config.DefaultApiKey, _config.DefaultEncryptionKey);
            var transaction = ParaTransaction(transacao, nerdsPagSvc);
            return ParaTransacao(await transaction.CancelAuthorization());
        }

        public static Transacao ParaTransacao(Transaction transaction)
        {
            return new Transacao
            {
                Id = Guid.NewGuid(),
                Status = (StatusTransacao)transaction.Status,
                ValorTotal = transaction.Amount,
                BandeiraCartao = transaction.CardBrand,
                CodigoAutorizacao = transaction.AuthorizationCode,
                CustoTransacao = transaction.Costs,
                DataTransacao = transaction.TransactionDate,
                NSU = transaction.Nsu,
                TID = transaction.Tid
            };
        }

        public static Transaction ParaTransaction(Transacao transacao, NerdsPagService nerdsPagService)
        {
            return new Transaction(nerdsPagService)
            {
                Status = (TransactionStatus)transacao.Status,
                Amount = transacao.ValorTotal,
                CardBrand = transacao.BandeiraCartao,
                AuthorizationCode = transacao.CodigoAutorizacao,
                Costs = transacao.CustoTransacao,
                Nsu = transacao.NSU,
                Tid = transacao.TID
            };
        }
    }
}
using System.Threading.Tasks;
using FluentValidation.Results;
using NerdStore.Enterprise.Pagamento.API.Facade;
using NerdStore.Enterprise.Pagamento.API.Models;
using NerdStore.Enterprise.Core.Messages.Integration;

namespace NerdStore.Enterprise.Pagamento.API.Services
{
    public class PagamentoService : IPagamentoService
    {
        public PagamentoService(IPagamentoFacade pagamentoFacade, IPagamentoRepository pagamentoRepository)
        {
            _pagamentoFacade = pagamentoFacade;
            _pagamentoRepository = pagamentoRepository;
        }

        private readonly IPagamentoFacade _pagamentoFacade;

        private readonly IPagamentoRepository _pagamentoRepository;
               
        public async Task<ResponseMessage> AutorizarPagamento(Models.Pagamento pagamento)
        {
            var transacao = await _pagamentoFacade.AutorizarPagamento(pagamento);
            var validationResult = new ValidationResult();

            if (transacao.Status != StatusTransacao.Autorizado)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", "Pagamento recusado, entre em contato com a sua operadora de cartão."));
                return new ResponseMessage(validationResult);
            }

            pagamento.AdicionarTransacao(transacao);
            _pagamentoRepository.AdicionarPagamento(pagamento);

            if (!await _pagamentoRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", "Houve um erro ao realizar o pagamento."));

                //TODO: Comunicar com o gateway para realizar o estorno.

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
    }
}
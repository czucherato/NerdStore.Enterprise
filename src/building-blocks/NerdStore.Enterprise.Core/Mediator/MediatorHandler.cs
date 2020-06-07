using MediatR;
using System.Threading.Tasks;
using FluentValidation.Results;
using NerdStore.Enterprise.Core.Messages;

namespace NerdStore.Enterprise.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        private readonly IMediator _mediator;

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }

        public async Task<ValidationResult> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }
    }
}
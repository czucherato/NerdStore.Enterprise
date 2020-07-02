using MediatR;
using System.Threading;
using System.Threading.Tasks;
using NerdStore.Enterprise.MessageBus;
using NerdStore.Enterprise.Core.Messages.Integration;

namespace NerdStore.Enterprise.Pedido.API.Application.Events
{
    public class PedidoEventHandler : INotificationHandler<PedidoRealizadoEvent>
    {
        public PedidoEventHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        private readonly IMessageBus _bus;

        public async Task Handle(PedidoRealizadoEvent message, CancellationToken cancellationToken)
        {
            await _bus.PublishAsync(new PedidoRealizadoIntegrationEvent(message.ClienteId));
        }
    }
}
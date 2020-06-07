using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Enterprise.Cliente.API.Application.Events
{
    public class ClienteEventHandler : INotificationHandler<ClienteRegistradoEvent>
    {
        public Task Handle(ClienteRegistradoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
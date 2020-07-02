using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using NerdStore.Enterprise.MessageBus;
using NerdStore.Enterprise.Carrinho.API.Data;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.Core.Messages.Integration;

namespace NerdStore.Enterprise.Carrinho.API.Services
{
    public class CarrinhoIntegrationHandler : BackgroundService
    {
        public CarrinhoIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        private readonly IMessageBus _bus;

        private readonly IServiceProvider _serviceProvider;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado", async request => await ApagarCarrinho(request));
        }

        private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarrinhoContext>();

            var carrinho = await context.CarrinhoCliente.FirstOrDefaultAsync(c => c.ClienteId == message.ClienteId);

            if (carrinho != null)
            {
                context.CarrinhoCliente.Remove(carrinho);
                await context.SaveChangesAsync();
            }
        }
    }
}
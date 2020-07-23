using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NerdStore.Enterprise.MessageBus;
using NerdStore.Enterprise.Core.DomainObjects;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.Pedido.Domain.Pedidos;
using NerdStore.Enterprise.Core.Messages.Integration;

namespace NerdStore.Enterprise.Pedido.API.Services
{
    public class PedidoIntegrationHandler : BackgroundService
    {
        public PedidoIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider)
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
            _bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", async request => await CancelarPedido(request));
            _bus.SubscribeAsync<PedidoPagoIntegrationEvent>("PedidoPago", async request => await FinalizarPedido(request));
        }

        private async Task FinalizarPedido(PedidoPagoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();
            var pedido = await pedidoRepository.ObterPorId(message.PedidoId);
            pedido.FinalizarPedido();
            pedidoRepository.Atualizar(pedido);

            if (!await pedidoRepository.UnitOfWork.Commit())
            {
                throw new DomainException($"Problema ao finalizar o pedido {message.PedidoId}");
            }
        }

        private async Task CancelarPedido(PedidoCanceladoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();
            var pedido = await pedidoRepository.ObterPorId(message.PedidoId);
            pedido.CancelarPedido();
            pedidoRepository.Atualizar(pedido);

            if (!await pedidoRepository.UnitOfWork.Commit())
            {
                throw new DomainException($"Problema ao finalizar o pedido {message.PedidoId}");
            }
        }
    }
}
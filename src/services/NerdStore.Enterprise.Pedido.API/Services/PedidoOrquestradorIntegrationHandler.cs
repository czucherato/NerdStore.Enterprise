using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NerdStore.Enterprise.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.Core.Messages.Integration;
using NerdStore.Enterprise.Pedido.API.Application.Queries;

namespace NerdStore.Enterprise.Pedido.API.Services
{
    public class PedidoOrquestradorIntegrationHandler : IHostedService, IDisposable
    {
        public PedidoOrquestradorIntegrationHandler(
            IServiceProvider serviceProvider,
            ILogger<PedidoOrquestradorIntegrationHandler> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<PedidoOrquestradorIntegrationHandler> _logger;

        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de pedidos iniciado.");
            _timer = new Timer(ProcessarPedidos, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }

        private async void ProcessarPedidos(object state)
        {
            _logger.LogInformation("Processando pedidos.");
            using var scope = _serviceProvider.CreateScope();
            var pedidoQueries = scope.ServiceProvider.GetRequiredService<IPedidoQueries>();
            var pedido = await pedidoQueries.ObterPedidosAutorizados();

            if (pedido == null) return;

            var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

            var pedidoAutorizado = new PedidoAutorizadoIntegrationEvent(pedido.ClienteId, pedido.Id,
                pedido.PedidoItems.ToDictionary(p => p.ProdutoId, p => p.Quantidade));

            await bus.PublishAsync(pedidoAutorizado);
            _logger.LogInformation($"Pedido ID: {pedido.Id} foi encaminhado para baixa no estoque.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Serviço de pedidos finalizado.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
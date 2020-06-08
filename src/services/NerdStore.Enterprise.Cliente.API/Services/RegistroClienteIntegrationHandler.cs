using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Hosting;
using NerdStore.Enterprise.MessageBus;
using NerdStore.Enterprise.Core.Mediator;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.Core.Messages.Integration;
using NerdStore.Enterprise.Cliente.API.Application.Commands;

namespace NerdStore.Enterprise.Cliente.API.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        public RegistroClienteIntegrationHandler(
            IMessageBus bus,
            IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageBus _bus;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            return Task.CompletedTask;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request => await RegistrarCliente(request));
            _bus.AdvancedBus.Connected += OnConnected;
        }

        private void OnConnected(object s, EventArgs e) => SetResponder();

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            ValidationResult sucesso;
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                sucesso = await mediator.EnviarComando(clienteCommand);
            }

            return new ResponseMessage(sucesso);
        }
    }
}
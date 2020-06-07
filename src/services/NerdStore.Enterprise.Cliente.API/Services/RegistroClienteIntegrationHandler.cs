using System;
using EasyNetQ;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Hosting;
using NerdStore.Enterprise.Core.Mediator;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.Core.Messages.Integration;
using NerdStore.Enterprise.Cliente.API.Application.Commands;

namespace NerdStore.Enterprise.Cliente.API.Services
{
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;
        private IBus _bus;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus = RabbitHutch.CreateBus("host=localhost:5672");
            _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request => new ResponseMessage(await RegistrarCliente(request)));
            return Task.CompletedTask;
        }

        private async Task<ValidationResult> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
            ValidationResult sucesso;
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                sucesso = await mediator.EnviarComando(clienteCommand);
            }

            return sucesso;
        }
    }
}
using MediatR;
using FluentValidation.Results;
using NerdStore.Enterprise.Core.Mediator;
using NerdStore.Enterprise.Cliente.API.Data;
using NerdStore.Enterprise.Cliente.API.Models;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.Cliente.API.Data.Repository;
using NerdStore.Enterprise.Cliente.API.Application.Events;
using NerdStore.Enterprise.Cliente.API.Application.Commands;

namespace NerdStore.Enterprise.Cliente.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ClienteContext>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();
            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
        }
    }
}
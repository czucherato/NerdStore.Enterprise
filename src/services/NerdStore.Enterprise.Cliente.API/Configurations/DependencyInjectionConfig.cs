using MediatR;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using NerdStore.Enterprise.Core.Mediator;
using NerdStore.Enterprise.Cliente.API.Data;
using NerdStore.Enterprise.Cliente.API.Models;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.WebAPI.Core.Usuario;
using NerdStore.Enterprise.Cliente.API.Data.Repository;
using NerdStore.Enterprise.Cliente.API.Application.Events;
using NerdStore.Enterprise.Cliente.API.Application.Commands;

namespace NerdStore.Enterprise.Cliente.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
            services.AddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult>, ClienteCommandHandler>();

            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClienteContext>();
        }
    }
}
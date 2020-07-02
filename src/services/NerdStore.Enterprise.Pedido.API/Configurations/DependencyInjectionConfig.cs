using MediatR;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using NerdStore.Enterprise.Core.Mediator;
using NerdStore.Enterprise.Pedido.Infra.Data;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.WebAPI.Core.Usuario;
using NerdStore.Enterprise.Pedido.Domain.Pedidos;
using NerdStore.Enterprise.Pedido.Domain.Vouchers;
using NerdStore.Enterprise.Pedido.Infra.Data.Repository;
using NerdStore.Enterprise.Pedido.API.Application.Events;
using NerdStore.Enterprise.Pedido.API.Application.Queries;
using NerdStore.Enterprise.Pedido.API.Application.Commands;

namespace NerdStore.Enterprise.Pedido.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // API
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Commands
            services.AddScoped<IRequestHandler<AdicionarPedidoCommand, ValidationResult>, PedidoCommandHandler>();

            //Events
            services.AddScoped<INotificationHandler<PedidoRealizadoEvent>, PedidoEventHandler>();

            // Application
            services.AddScoped<IPedidoQueries, PedidoQueries>();
            services.AddScoped<IVoucherQueries, VoucherQueries>();
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Data
            services.AddScoped<PedidosContext>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
        }
    }
}
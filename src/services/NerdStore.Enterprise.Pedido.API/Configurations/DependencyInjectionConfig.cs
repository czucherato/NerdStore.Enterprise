using Microsoft.AspNetCore.Http;
using NerdStore.Enterprise.Core.Mediator;
using NerdStore.Enterprise.Pedido.Infra.Data;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.WebAPI.Core.Usuario;
using NerdStore.Enterprise.Pedido.Domain.Vouchers;
using NerdStore.Enterprise.Pedido.Infra.Data.Repository;
using NerdStore.Enterprise.Pedido.API.Application.Queries;

namespace NerdStore.Enterprise.Pedido.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // API
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Application
            services.AddScoped<IVoucherQueries, VoucherQueries>();
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Data
            services.AddScoped<PedidosContext>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
        }
    }
}
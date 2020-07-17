using Microsoft.AspNetCore.Http;
using NerdStore.Enterprise.Pagamento.API.Data;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.WebAPI.Core.Usuario;
using NerdStore.Enterprise.Pagamento.API.Models;
using NerdStore.Enterprise.Pagamento.API.Data.Repository;

namespace NerdStore.Enterprise.Pagamento.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // API
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Data
            services.AddScoped<PagamentoContext>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        }
    }
}
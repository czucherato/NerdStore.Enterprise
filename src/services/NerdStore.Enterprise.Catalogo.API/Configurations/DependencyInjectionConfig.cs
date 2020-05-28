using NerdStore.Enterprise.Catalogo.API.Data;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.Catalogo.API.Models;
using NerdStore.Enterprise.Catalogo.API.Data.Repository;

namespace NerdStore.Enterprise.Catalogo.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<CatalogoContext>();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.WebApp.MVC.Services;

namespace NerdStore.Enterprise.WebApp.MVC.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
        }
    }
}
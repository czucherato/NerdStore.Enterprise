using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.WebApp.MVC.Services;
using NerdStore.Enterprise.WebApp.MVC.Extensions;

namespace NerdStore.Enterprise.WebApp.MVC.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
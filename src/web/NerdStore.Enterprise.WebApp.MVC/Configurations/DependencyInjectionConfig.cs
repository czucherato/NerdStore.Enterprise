using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.WebApp.MVC.Services;
using NerdStore.Enterprise.WebApp.MVC.Extensions;
using NerdStore.Enterprise.WebApp.MVC.Services.Handlers;

namespace NerdStore.Enterprise.WebApp.MVC.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
            //services.AddHttpClient<ICatalogoService, CatalogoService>().AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddHttpClient("Refit", options =>
            {
                options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
            })
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
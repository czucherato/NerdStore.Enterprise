using Polly;
using System;
using Microsoft.AspNetCore.Http;
using NerdStore.Enterprise.WebAPI.Core.Usuario;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.BFF.Compras.Services;
using NerdStore.Enterprise.BFF.Compras.Extensions;
using NerdStore.Enterprise.WebAPI.Core.Extensions;

namespace NerdStore.Enterprise.BFF.Compras.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services
                .AddHttpClient<IPedidoService, PedidoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services
                .AddHttpClient<ICarrinhoService, CarrinhoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            services
                .AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
        }
    }
}
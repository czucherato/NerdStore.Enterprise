using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.BFF.Compras.Services.gRPC;
using NerdStore.Enterprise.Carrinho.API.Services.gRPC;

namespace NerdStore.Enterprise.BFF.Compras.Configurations
{
    public static class GrpcConfig
    {
        public static void ConfigureGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GrpcServiceInterceptor>();
            services.AddScoped<ICarrinhoGrpcService, CarrinhoGrpcService>();
            services.AddGrpcClient<CarrinhoCompras.CarrinhoComprasClient>(options =>
            {
                options.Address = new Uri(configuration["CarrinhoUrl"]);
            }).AddInterceptor<GrpcServiceInterceptor>();
        }
    }
}
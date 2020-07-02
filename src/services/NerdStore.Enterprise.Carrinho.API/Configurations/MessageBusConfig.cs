using NerdStore.Enterprise.Core.Utils;
using NerdStore.Enterprise.MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.Carrinho.API.Services;

namespace NerdStore.Enterprise.Carrinho.API.Configurations
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<CarrinhoIntegrationHandler>();
        }
    }
}
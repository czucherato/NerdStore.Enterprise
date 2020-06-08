using System;
using Microsoft.Extensions.DependencyInjection;

namespace NerdStore.Enterprise.MessageBus
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException();

            services.AddSingleton<IMessageBus>(new MessageBus(connectionString));

            return services;
        }
    }
}
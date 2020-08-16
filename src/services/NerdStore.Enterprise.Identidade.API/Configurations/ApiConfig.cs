using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.WebAPI.Core.Identidade;
using Jwks.Manager.AspNetCore;
using NerdStore.Enterprise.WebAPI.Core.Usuario;

namespace NerdStore.Enterprise.Identidade.API.Configurations
{
    public static  class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IAspNetUser, AspNetUser>();
            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseJwksDiscovery("/minha-chave");
            app.UseJwksDiscovery();

            return app;
        }
    }
}
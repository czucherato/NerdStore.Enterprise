using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using NerdStore.Enterprise.Pagamento.API.Data;
using Microsoft.Extensions.DependencyInjection;
using NerdStore.Enterprise.Pagamento.API.Facade;
using NerdStore.Enterprise.WebAPI.Core.Identidade;

namespace NerdStore.Enterprise.Pagamento.API.Configurations
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PagamentoContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddCors(options => options.AddPolicy("Total", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.Configure<PagamentoConfig>(configuration.GetSection("PagamentoConfig"));
            services.AddControllers();
        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthConfiguration();
            app.UseCors("Total");
            app.UseEndpoints(endPoints => endPoints.MapControllers());
        }
    }
}
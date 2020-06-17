using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using NerdStore.Enterprise.BFF.Compras.Extensions;

namespace NerdStore.Enterprise.BFF.Compras.Services
{
    public class PedidoService : Service, IPedidoService
    {
        public PedidoService(
            HttpClient httpClient,
            IOptions<AppServiceSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
        }

        private readonly HttpClient _httpClient;
    }
}
using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using NerdStore.Enterprise.BFF.Compras.Extensions;

namespace NerdStore.Enterprise.BFF.Compras.Services
{
    public class PagamentoService : Service, IPagamentoService
    {
        public PagamentoService(
            HttpClient httpClient,
            IOptions<AppServiceSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PagamentoUrl);
        }

        private readonly HttpClient _httpClient;
    }
}
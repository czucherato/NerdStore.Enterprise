using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NerdStore.Enterprise.BFF.Compras.Models;
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

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var response = await _httpClient.GetAsync($"/voucher/{codigo}/");
            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<VoucherDTO>(response);
        }
    }
}
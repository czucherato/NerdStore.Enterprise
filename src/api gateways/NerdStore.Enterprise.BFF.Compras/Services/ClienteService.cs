using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NerdStore.Enterprise.BFF.Compras.Models;
using NerdStore.Enterprise.BFF.Compras.Extensions;

namespace NerdStore.Enterprise.BFF.Compras.Services
{
    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(HttpClient httpClient, IOptions<AppServiceSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
        }

        public async Task<EnderecoDTO> ObterEndereco()
        {
            var response = await _httpClient.GetAsync("/cliente/endereco/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EnderecoDTO>(response);
        }
    }
}
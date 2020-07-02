using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.Core.Communication;
using NerdStore.Enterprise.WebApp.MVC.Extensions;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
        }

        public async Task<EnderecoViewModel> ObterEndereco()
        {
            var response = await _httpClient.GetAsync("/cliente/endereco/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EnderecoViewModel>(response);
        }

        public async Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco)
        {
            var enderecoContent = ObterConteudo(endereco);

            var response = await _httpClient.PostAsync("/cliente/endereco/", enderecoContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }
    }
}
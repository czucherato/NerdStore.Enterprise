using Microsoft.Extensions.Options;
using NerdStore.Enterprise.WebApp.MVC.Extensions;
using NerdStore.Enterprise.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public class CatalogoService : Service, ICatalogoService
    {
        public CatalogoService(
            HttpClient httpClient,
            IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
        }

        private readonly HttpClient _httpClient;

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var response = await _httpClient.GetAsync("/catalogo/produtos");
            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");
            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<ProdutoViewModel>(response);
        }
    }
}
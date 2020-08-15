using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.WebApp.MVC.Extensions;

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

        public async Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string query = null)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos?ps={pageSize}&page={pageIndex}&q={query}");
            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<PagedViewModel<ProdutoViewModel>>(response);
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");
            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<ProdutoViewModel>(response);
        }
    }
}
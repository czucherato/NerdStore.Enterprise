using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.Core.Communication;
using NerdStore.Enterprise.WebApp.MVC.Extensions;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public class ComprasBffService : Service, IComprasBffService
    {
        public ComprasBffService(
            HttpClient httpClient,
            IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ComprasBffUrl);
        }

        private readonly HttpClient _httpClient;

        public async Task<CarrinhoViewModel> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync("/compras/carrinho/");
            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
        }

        public async Task<int> ObterQuantidadeCarrinho()
        {
            var response = await _httpClient.GetAsync("/compras/carrinho-quantidade/");
            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<int>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);
            var response = await _httpClient.PostAsync("/compras/carrinho/items/", itemContent);
            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);
            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);
            var response = await _httpClient.PutAsync($"/compras/carrinho/items/{produto.ProdutoId}", itemContent);
            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);
            return RetornoOk();
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/compras/carrinho/items/{produtoId}");
            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);
            return RetornoOk();
        }
    }
}
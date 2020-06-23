using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NerdStore.Enterprise.BFF.Compras.Models;
using NerdStore.Enterprise.Core.Communication;
using NerdStore.Enterprise.BFF.Compras.Extensions;

namespace NerdStore.Enterprise.BFF.Compras.Services
{
    public class CarrinhoService : Service, ICarrinhoService
    {
        public CarrinhoService(
            HttpClient httpClient,
            IOptions<AppServiceSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
        }

        private readonly HttpClient _httpClient;

        public async Task<CarrinhoDTO> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync("/carrinho/");
            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<CarrinhoDTO>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO produto)
        {
            var itemContent = ObterConteudo(produto);
            var response = await _httpClient.PostAsync("/carrinho/", itemContent);
            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);
            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO produto)
        {
            var itemContent = ObterConteudo(produto);
            var response = await _httpClient.PutAsync($"/carrinho/{produto.ProdutoId}", itemContent);
            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);
            return RetornoOk();
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/carrinho/{produtoId}");
            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);
            return RetornoOk();
        }

        public async Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher)
        {
            var itemContent = ObterConteudo(voucher);
            var response = await _httpClient.PostAsync("/carrinho/aplicar-voucher/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }
    }
}
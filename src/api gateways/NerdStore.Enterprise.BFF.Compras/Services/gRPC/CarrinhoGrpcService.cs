using System;
using System.Threading.Tasks;
using NerdStore.Enterprise.BFF.Compras.Models;
using NerdStore.Enterprise.Carrinho.API.Services.gRPC;

namespace NerdStore.Enterprise.BFF.Compras.Services.gRPC
{
    public class CarrinhoGrpcService : ICarrinhoGrpcService
    {
        public CarrinhoGrpcService(CarrinhoCompras.CarrinhoComprasClient carrinhoComprasClient)
        {
            _carrinhoComprasClient = carrinhoComprasClient;
        }

        private readonly CarrinhoCompras.CarrinhoComprasClient _carrinhoComprasClient;

        public async Task<CarrinhoDTO> ObterCarrinho()
        {
            var response = await _carrinhoComprasClient.ObterCarrinhoAsync(new ObterCarrinhoRequest());
            return MapCarrinhoClienteProtoResponseToDTO(response);
        }

        private static CarrinhoDTO MapCarrinhoClienteProtoResponseToDTO(CarrinhoClienteResponse carrinhoClienteResponse)
        {
            var carrinhoDTO = new CarrinhoDTO
            {
                ValorTotal = (decimal)carrinhoClienteResponse.Valortotal,
                Desconto = (decimal)carrinhoClienteResponse.Desconto,
                VoucherUtilizado = carrinhoClienteResponse.Voucherutilizado
            };

            if (carrinhoClienteResponse.Voucher != null)
            {
                carrinhoDTO.Voucher = new VoucherDTO
                {
                    Codigo = carrinhoClienteResponse.Voucher.Codigo,
                    Percentual = (decimal?)carrinhoClienteResponse.Voucher.Percentual,
                    ValorDesconto = (decimal?)carrinhoClienteResponse.Voucher.Valordesconto,
                    TipoDesconto = carrinhoClienteResponse.Voucher.Tipodesconto
                };
            }

            foreach (var item in carrinhoClienteResponse.Itens)
            {
                carrinhoDTO.Itens.Add(new ItemCarrinhoDTO
                {
                    Nome = item.Nome,
                    Imagem = item.Imagem,
                    ProdutoId = Guid.Parse(item.Id),
                    Quantidade = item.Quantidade,
                    Valor = (decimal)item.Valor
                });
            }

            return carrinhoDTO;
        }
    }
}
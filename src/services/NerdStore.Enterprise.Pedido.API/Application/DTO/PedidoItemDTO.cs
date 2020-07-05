using System;
using NerdStore.Enterprise.Pedido.Domain.Pedidos;

namespace NerdStore.Enterprise.Pedido.API.Application.DTO
{
    public class PedidoItemDTO
    {
        public Guid ProdutoId { get; set; }

        public Guid PedidoId { get; set; }

        public string Nome { get; set; }

        public string Imagem { get; set; }

        public int Quantidade { get; set; }

        public decimal Valor { get; set; }

        public static PedidoItem ParaPedidoItem(PedidoItemDTO pedidoItemDTO)
        {
            return new PedidoItem(pedidoItemDTO.ProdutoId, pedidoItemDTO.Nome, pedidoItemDTO.Quantidade, pedidoItemDTO.Valor, pedidoItemDTO.Imagem);
        }
    }
}
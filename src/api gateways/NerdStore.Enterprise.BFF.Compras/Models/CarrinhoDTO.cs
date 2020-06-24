using System.Collections.Generic;

namespace NerdStore.Enterprise.BFF.Compras.Models
{
    public class CarrinhoDTO
    {
        public decimal ValorTotal { get; set; }

        public decimal Desconto { get; set; }

        public bool VoucherUtilizado { get; set; }

        public VoucherDTO Voucher { get; set; }

        public List<ItemCarrinhoDTO> Itens { get; set; } = new List<ItemCarrinhoDTO>();
    }
}
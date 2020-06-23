using System;
using NerdStore.Enterprise.Core.DomainObjects;
using NerdStore.Enterprise.Pedido.Domain.Vouchers.Specs;

namespace NerdStore.Enterprise.Pedido.Domain.Vouchers
{
    public class Voucher : Entity, IAggregateRoot
    {
        public string Codigo { get; private set; }

        public decimal? Percentual { get; private set; }

        public decimal? ValorDesconto { get; private set; }

        public int Quantidade { get; private set; }

        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public DateTime? DataUtilizacao { get; private set; }

        public DateTime DataValidade { get; private set; }

        public bool Ativo { get; private set; }

        public bool Utilizado { get; private set; }

        public bool EstaValidoParaUtilizacao()
        {
            return new VoucherAtivoSpecification()
                .And(new VoucherDataSpecification())
                .And(new VoucherUtilizadoSpecification())
                .And(new VoucherQuantidadeSpecification())
                .IsSatisfiedBy(this);
        }

        public void MarcarComoUtilizado()
        {
            Ativo = false;
            Utilizado = true;
            Quantidade = 0;
        }
    }
}
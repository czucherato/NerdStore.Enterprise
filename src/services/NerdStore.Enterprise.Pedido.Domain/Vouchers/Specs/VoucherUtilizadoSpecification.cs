using System;
using System.Linq.Expressions;
using NerdStore.Enterprise.Core.Specification;

namespace NerdStore.Enterprise.Pedido.Domain.Vouchers.Specs
{
    public class VoucherUtilizadoSpecification : Specification<Voucher>
    {
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => !voucher.Utilizado;
        }
    }
}
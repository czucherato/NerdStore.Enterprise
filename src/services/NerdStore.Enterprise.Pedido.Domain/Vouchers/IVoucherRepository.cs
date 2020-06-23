using System.Threading.Tasks;
using NerdStore.Enterprise.Core.Data;

namespace NerdStore.Enterprise.Pedido.Domain.Vouchers
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}
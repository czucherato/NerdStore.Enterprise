using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NerdStore.Enterprise.Core.Data;
using NerdStore.Enterprise.Pedido.Domain.Vouchers;

namespace NerdStore.Enterprise.Pedido.Infra.Data.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly PedidosContext _context;

        public VoucherRepository(PedidosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Atualizar(Voucher voucher)
        {
            _context.Update(voucher);
        }

        public async Task<Voucher> ObterVoucherPorCodigo(string codigo)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
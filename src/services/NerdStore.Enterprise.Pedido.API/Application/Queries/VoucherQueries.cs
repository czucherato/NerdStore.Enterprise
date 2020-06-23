using System.Threading.Tasks;
using NerdStore.Enterprise.Pedido.Domain.Vouchers;
using NerdStore.Enterprise.Pedido.API.Application.DTO;

namespace NerdStore.Enterprise.Pedido.API.Application.Queries
{
    public class VoucherQueries : IVoucherQueries
    {
        public VoucherQueries(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        private readonly IVoucherRepository _voucherRepository;

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var voucher = await _voucherRepository.ObterVoucherPorCodigo(codigo);

            if (voucher == null) return null;
            if (!voucher.EstaValidoParaUtilizacao()) return null;

            return new VoucherDTO
            {
                Codigo = voucher.Codigo,
                TipoDesconto = (int)voucher.TipoDescontoVoucher,
                Percentual = voucher.Percentual,
                ValorDesconto = voucher.ValorDesconto
            };
        }
    }
}
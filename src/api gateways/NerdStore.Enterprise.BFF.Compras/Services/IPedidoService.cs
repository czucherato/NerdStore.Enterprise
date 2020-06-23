using System.Threading.Tasks;
using NerdStore.Enterprise.BFF.Compras.Models;

namespace NerdStore.Enterprise.BFF.Compras.Services
{
    public interface IPedidoService
    {
        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }
}
using NerdStore.Enterprise.Pedido.API.Application.DTO;
using System.Threading.Tasks;

namespace NerdStore.Enterprise.Pedido.API.Application.Queries
{
    public interface IVoucherQueries
    {
        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }
}
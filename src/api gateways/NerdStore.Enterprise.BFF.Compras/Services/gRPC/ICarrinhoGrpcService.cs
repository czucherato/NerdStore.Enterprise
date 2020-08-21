using System.Threading.Tasks;
using NerdStore.Enterprise.BFF.Compras.Models;

namespace NerdStore.Enterprise.BFF.Compras.Services.gRPC
{
    public interface ICarrinhoGrpcService
    {
        Task<CarrinhoDTO> ObterCarrinho();
    }
}
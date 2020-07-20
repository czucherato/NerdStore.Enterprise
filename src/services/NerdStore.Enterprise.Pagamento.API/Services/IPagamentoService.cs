using System.Threading.Tasks;
using NerdStore.Enterprise.Core.Messages.Integration;

namespace NerdStore.Enterprise.Pagamento.API.Services
{
    public interface IPagamentoService
    {
        Task<ResponseMessage> AutorizarPagamento(Models.Pagamento pagamento);
    }
}
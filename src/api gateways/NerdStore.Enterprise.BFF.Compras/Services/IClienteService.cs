using NerdStore.Enterprise.BFF.Compras.Models;
using System.Threading.Tasks;

namespace NerdStore.Enterprise.BFF.Compras.Services
{
    public interface IClienteService
    {
        Task<EnderecoDTO> ObterEndereco();
    }
}
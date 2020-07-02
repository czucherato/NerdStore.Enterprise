using System.Threading.Tasks;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.Core.Communication;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public interface IClienteService
    {
        Task<EnderecoViewModel> ObterEndereco();
        Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco);
    }
}
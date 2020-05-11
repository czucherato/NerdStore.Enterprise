using System.Threading.Tasks;
using NerdStore.Enterprise.WebApp.MVC.Models;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<string> Login(UsuarioLoginViewModel parametros);

        Task<string> Registro(UsuarioRegistroViewModel parametros);
    }
}
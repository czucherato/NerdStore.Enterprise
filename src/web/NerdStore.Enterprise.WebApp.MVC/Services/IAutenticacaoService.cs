using System.Threading.Tasks;
using NerdStore.Enterprise.WebApp.MVC.Models;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<UsuarioRespostaLogin> Login(UsuarioLoginViewModel parametros);

        Task<UsuarioRespostaLogin> Registro(UsuarioRegistroViewModel parametros);
    }
}
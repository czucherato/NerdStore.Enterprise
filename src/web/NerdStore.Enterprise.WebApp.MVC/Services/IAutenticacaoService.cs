using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using NerdStore.Enterprise.WebApp.MVC.Models;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<UsuarioRespostaLogin> Login(UsuarioLoginViewModel parametros);

        Task<UsuarioRespostaLogin> Registro(UsuarioRegistroViewModel parametros);

        Task RealizarLogin(UsuarioRespostaLogin usuario);

        Task Logout();

        JwtSecurityToken ObterJwtToken(string jwtToken);

        bool TokenExpirado();

        Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken);

        Task<bool> RefreshTokenValido();
    }
}
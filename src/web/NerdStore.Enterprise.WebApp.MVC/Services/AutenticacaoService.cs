using System.Net.Http;
using System.Threading.Tasks;
using NerdStore.Enterprise.WebApp.MVC.Models;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly HttpClient _httpClient;

        public async Task<UsuarioRespostaLogin> Login(UsuarioLoginViewModel parametros)
        {
            var response = await _httpClient.PostAsync("https://localhost:44330/api/identidade/autenticar", ObterConteudo(parametros));

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistroViewModel parametros)
        {
            var response = await _httpClient.PostAsync("https://localhost:44330/api/identidade/nova-conta", ObterConteudo(parametros));

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }
    }
}
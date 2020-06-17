using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NerdStore.Enterprise.WebApp.MVC.Models;
using NerdStore.Enterprise.Core.Communication;
using NerdStore.Enterprise.WebApp.MVC.Extensions;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        public AutenticacaoService(
            HttpClient httpClient,
            IOptions<AppSettings> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.AutenticacaoUrl);
        }

        private readonly HttpClient _httpClient;

        public async Task<UsuarioRespostaLogin> Login(UsuarioLoginViewModel parametros)
        {
            var response = await _httpClient.PostAsync("/api/identidade/autenticar", ObterConteudo(parametros));

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
            var response = await _httpClient.PostAsync("/api/identidade/nova-conta", ObterConteudo(parametros));

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
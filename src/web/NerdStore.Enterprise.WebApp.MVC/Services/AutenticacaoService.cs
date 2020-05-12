using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using NerdStore.Enterprise.WebApp.MVC.Models;

namespace NerdStore.Enterprise.WebApp.MVC.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public async Task<UsuarioRespostaLogin> Login(UsuarioLoginViewModel parametros)
        {
            var content = new StringContent(JsonSerializer.Serialize(parametros), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:44330/api/identidade/autenticar", content);

            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), _options);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistroViewModel parametros)
        {
            var content = new StringContent(JsonSerializer.Serialize(parametros), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:44330/api/identidade/nova-conta", content);

            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), _options);
        }
    }
}
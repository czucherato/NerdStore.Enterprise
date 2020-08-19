using System;

namespace NerdStore.Enterprise.Identidade.API.Models
{
    public class UsuarioRespostaLogin
    {
        public string AccessToken { get; set; }

        public Guid RefreshToken { get; set; }

        public double ExpiresIn { get; set; }

        public UsuarioToken UsuarioToken { get; set; }
    }
}
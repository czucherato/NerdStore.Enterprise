using NerdStore.Enterprise.Core.Communication;
using System;

namespace NerdStore.Enterprise.WebApp.MVC.Models
{
    public class UsuarioRespostaLogin
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public double ExpiresIn { get; set; }

        public UsuarioToken UsuarioToken { get; set; }

        public ResponseResult ResponseResult { get; set; }
    }
}
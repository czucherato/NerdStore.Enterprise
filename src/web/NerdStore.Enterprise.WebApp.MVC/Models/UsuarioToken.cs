using System.Collections.Generic;

namespace NerdStore.Enterprise.WebApp.MVC.Models
{
    public class UsuarioToken
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public IEnumerable<UsuarioClaim> Claims { get; set; }
    }
}
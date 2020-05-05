using System.Collections.Generic;

namespace NerdStore.Enterprise.Identidade.API.Models
{
    public class UsuarioToken
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public IEnumerable<UsuarioClaim> Claims { get; set; }
    }
}
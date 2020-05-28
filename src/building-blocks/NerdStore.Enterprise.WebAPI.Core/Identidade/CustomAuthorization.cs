using System.Linq;
using Microsoft.AspNetCore.Http;

namespace NerdStore.Enterprise.WebAPI.Core.Identidade
{
    public class CustomAuthorization
    {
        public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated
                && context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }
    }
}
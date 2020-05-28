using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NerdStore.Enterprise.WebAPI.Core.Identidade
{
    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        private readonly Claim _claim;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            if (!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
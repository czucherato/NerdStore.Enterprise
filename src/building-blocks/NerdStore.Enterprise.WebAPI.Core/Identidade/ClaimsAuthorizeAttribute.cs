﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace NerdStore.Enterprise.WebAPI.Core.Identidade
{
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue)
            : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }
}
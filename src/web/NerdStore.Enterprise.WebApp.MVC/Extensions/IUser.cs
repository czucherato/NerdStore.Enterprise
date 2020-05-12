using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace NerdStore.Enterprise.WebApp.MVC.Extensions
{
    public interface IUser
    {
        string Name { get; }

        Guid ObterUserId();

        string ObterUserEmail();

        string ObterUserToken();

        bool EstaAutenticado();

        bool PossuiRole(string role);

        IEnumerable<Claim> ObterClaims();

        HttpContext ObterHttpContext();
    }
}
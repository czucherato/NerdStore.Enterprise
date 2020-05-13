using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NerdStore.Enterprise.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        public async Task InvokeAsync(HttpContext context)
        {
            try { await _next(context); }
            catch (CustomHttpRequestException ex) { HandleRequestExceptionAsync(context, ex); }
        }

        private static void HandleRequestExceptionAsync(HttpContext context, CustomHttpRequestException httpRequestException)
        {
            if (httpRequestException.StatusCode == HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect("/login");
                return;
            }

            context.Response.StatusCode = (int)httpRequestException.StatusCode;
        }
    }
}
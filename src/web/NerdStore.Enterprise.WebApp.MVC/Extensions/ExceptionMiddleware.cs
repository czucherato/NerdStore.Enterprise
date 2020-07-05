using Refit;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;

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
            catch (CustomHttpRequestException ex) 
            { 
                HandleRequestExceptionAsync(context, ex.StatusCode); 
            }
            catch (ValidationApiException ex) 
            {
                HandleRequestExceptionAsync(context, ex.StatusCode); 
            }
            catch (ApiException ex) 
            {
                HandleRequestExceptionAsync(context, ex.StatusCode); 
            }
            catch (BrokenCircuitException) 
            { 
                HandleCircuitBreakerExceptionAsync(context); 
            }
        }

        private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                return;
            }

            context.Response.StatusCode = (int)statusCode;
        }

        private static void HandleCircuitBreakerExceptionAsync(HttpContext context)
        {
            context.Response.Redirect("/sistema-indisponivel");
        }
    }
}
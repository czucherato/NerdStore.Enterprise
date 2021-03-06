﻿using Refit;
using System;
using Grpc.Core;
using System.Net;
using Polly.CircuitBreaker;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NerdStore.Enterprise.WebApp.MVC.Services;

namespace NerdStore.Enterprise.WebApp.MVC.Extensions
{
    public class ExceptionMiddleware
    {
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        private static IAutenticacaoService _autenticacaoService;

        public async Task InvokeAsync(HttpContext context, IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;

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
            catch (RpcException ex)
            {
                //400 Bad Request  INTERNAL
                //401 Unauthorized UNAUTHENTICATED
                //403 Forbidden    PERMISSION_DENIED
                //404 Not Found    UNIMPLEMENTED

                var statusCode = ex.StatusCode switch
                {
                    StatusCode.Internal => 400,
                    StatusCode.Unauthenticated => 401,
                    StatusCode.PermissionDenied => 403,
                    StatusCode.Unimplemented => 404,
                    _ => 500
                };

                var httpStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCode.ToString());
                HandleRequestExceptionAsync(context, httpStatusCode);
            }
        }

        private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                if (_autenticacaoService.TokenExpirado())
                {
                    if (_autenticacaoService.RefreshTokenValido().Result)
                    {
                        context.Response.Redirect(context.Request.Path);
                        return;
                    }
                }

                _autenticacaoService.Logout();
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
using System;
using System.Net;

namespace NerdStore.Enterprise.WebApp.MVC.Extensions
{
    public class CustomHttpRequestException : Exception
    {
        public CustomHttpRequestException() {}

        public CustomHttpRequestException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public CustomHttpRequestException(string message, Exception innerException)
            : base(message, innerException) {}

        public HttpStatusCode StatusCode { get; private set; }
    }
}
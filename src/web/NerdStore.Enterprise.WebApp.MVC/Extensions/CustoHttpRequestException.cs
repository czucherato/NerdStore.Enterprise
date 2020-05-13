using System;
using System.Net;

namespace NerdStore.Enterprise.WebApp.MVC.Extensions
{
    public class CustoHttpRequestException : Exception
    {
        public CustoHttpRequestException() {}

        public CustoHttpRequestException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public CustoHttpRequestException(string message, Exception innerException)
            : base(message, innerException) {}

        public HttpStatusCode StatusCode { get; private set; }
    }
}
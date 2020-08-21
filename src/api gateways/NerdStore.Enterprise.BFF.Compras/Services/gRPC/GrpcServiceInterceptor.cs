using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;

namespace NerdStore.Enterprise.BFF.Compras.Services.gRPC
{
    public class GrpcServiceInterceptor : Interceptor
    {
        public GrpcServiceInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var headers = new Metadata
            {
                { "Authorization", token }
            };

            var options = context.Options.WithHeaders(headers);
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);

            return base.AsyncUnaryCall(request, context, continuation);
        }
    }
}
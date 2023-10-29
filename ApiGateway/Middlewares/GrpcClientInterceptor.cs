using Grpc.Core;
using Grpc.Core.Interceptors;

namespace ApiGateway.Middlewares
{

    public class GrpcClientInterceptor : Interceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GrpcClientInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var jwtToken = httpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(jwtToken))
            {
                var metadata = context.Options.Headers ?? new Metadata();
                metadata.Add("Authorization", $"Bearer {jwtToken}");
                var optionsWithMetadata = context.Options.WithHeaders(metadata);

                var newContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, optionsWithMetadata);
                return continuation(request, newContext);
            }

            return continuation(request, context);
        }

    }






}
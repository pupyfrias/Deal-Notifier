using Grpc.Core;
using Grpc.Core.Interceptors;
using ILogger = Serilog.ILogger;

namespace Identity.GrpcService.Interceptors
{
    public class ErrorHandlingInterceptor : Interceptor
    {
        private readonly ILogger _logger;
        public ErrorHandlingInterceptor(ILogger logger) 
        {
            _logger = logger;
        
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (RpcException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.Error($"Exception thrown on UnaryServerHandler: Exception {e}. InnerException: {e.InnerException}");
                throw new RpcException(new Status(StatusCode.Unknown, e.InnerException?.Message ?? e.Message, e));
            }
        }
    }

}
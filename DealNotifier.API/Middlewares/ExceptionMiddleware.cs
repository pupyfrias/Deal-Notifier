using DealNotifier.Core.Application.Exceptions;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.Wrappers;
using Grpc.Core;
using Serilog.Context;
using ILogger = Serilog.ILogger;

namespace DealNotifier.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                Exception exception = e.InnerException ?? e;

                #region StatusCode

                string message = exception.Message;
                switch (exception)
                {
                    case BadRequestException:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;

                    case NotFoundException:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;

                    case RpcException rpcE:

                        message = rpcE.Status.Detail;
                        switch (rpcE.StatusCode)
                        {
                            case StatusCode.OK:
                                context.Response.StatusCode = StatusCodes.Status200OK;
                                break;

                            case StatusCode.Unauthenticated:
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                break;

                            case StatusCode.PermissionDenied:
                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                break;

                            case StatusCode.NotFound:
                                context.Response.StatusCode = StatusCodes.Status404NotFound;
                                break;

                            case StatusCode.AlreadyExists:
                            case StatusCode.Aborted:
                                context.Response.StatusCode = StatusCodes.Status409Conflict;
                                break;

                            case StatusCode.InvalidArgument:
                            case StatusCode.OutOfRange:
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                break;

                            case StatusCode.FailedPrecondition:
                                context.Response.StatusCode = StatusCodes.Status412PreconditionFailed;
                                break;

                            case StatusCode.Cancelled:
                                context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                                break;

                            case StatusCode.ResourceExhausted:
                                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                                break;

                            case StatusCode.DeadlineExceeded:
                                context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
                                break;

                            case StatusCode.Unimplemented:
                                context.Response.StatusCode = StatusCodes.Status501NotImplemented;
                                break;

                            case StatusCode.Unavailable:
                                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                                break;

                            default:
                                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                break;
                        }
                        break;

                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                #endregion StatusCode

                var userName = context.GetUserName();
                LogContext.PushProperty("UserName", userName);
                _logger.Error(e, e.Message);

                var response = new Response<string>(message);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
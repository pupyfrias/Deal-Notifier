using Catalog.Application.Extensions;
using Serilog.Context;

namespace DealNotifier.API.Middlewares
{
    public class SerilogMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userName = context.GetUserName();
            LogContext.PushProperty("UserName", userName);

            await _next(context);
        }
    }
}
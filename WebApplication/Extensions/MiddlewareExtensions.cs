using WebApi.Middlewares;
namespace WebApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void AddMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}

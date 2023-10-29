using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;
using Serilog;
using Microsoft.AspNetCore.Http.Extensions;

namespace ApiGateway.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class  RequirePermissionBaseAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permission;


        protected RequirePermissionBaseAttribute(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var userName = user.Identity?.Name;
            var currentController = context.RouteData.Values["controller"]?.ToString();
            var url = context.HttpContext.Request.GetDisplayUrl();

            var requiredClaim = $"{currentController}_{_permission}";

            if (!user.HasClaim("ResourcePermission", requiredClaim))
            {
                context.Result = new ForbidResult();

                Log.Information($"{userName} tried access to \"{url}\" without having permission");

            }
        }
    }


}
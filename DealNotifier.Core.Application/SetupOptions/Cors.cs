using Microsoft.AspNetCore.Cors.Infrastructure;

namespace DealNotifier.Core.Application.SetupOptions
{
    public static class Cors
    {
        public static Action<CorsOptions> Options = options =>
        {
            options.AddPolicy("AllowAll", policy =>
               policy.WithOrigins("http://localhost", "https://localhost")
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials());
        };
    }
}
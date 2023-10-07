using Microsoft.AspNetCore.Cors.Infrastructure;

namespace DealNotifier.Core.Application.Setups
{
    public static class CorsSetup
    {
        public static readonly Action<CorsOptions> Configure = options =>
        {
            options.AddPolicy("AllowAll", policy =>
               policy.WithOrigins("http://localhost")
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials());
        };
    }
}
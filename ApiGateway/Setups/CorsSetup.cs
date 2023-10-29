using Microsoft.AspNetCore.Cors.Infrastructure;

namespace ApiGateway.Setups
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
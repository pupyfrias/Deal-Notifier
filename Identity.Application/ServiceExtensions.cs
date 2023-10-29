using Identity.Application.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Identity.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            
            RSA rsa = RSA.Create();
            services.AddSingleton(rsa);
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                var publicKey = new RsaSecurityKey(rsa);
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(1),
                    IssuerSigningKey = publicKey,
                    ValidateIssuerSigningKey = false,
                    ValidAudience = configuration["JWTConfig:Audience"],
                    ValidIssuer = configuration["JWTConfig:Issuer"],
                    

                };

            });


            services.AddAuthorization();
            services.Configure<JwtConfig>(configuration.GetSection("JWTConfig"));
            services.AddMemoryCache();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
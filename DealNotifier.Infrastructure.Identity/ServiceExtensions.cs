using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.Wrappers;
using DealNotifier.Core.Domain.Configs;
using DealNotifier.Infrastructure.Identity.DbContext;
using DealNotifier.Infrastructure.Identity.Models;
using DealNotifier.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DealNotifier.Infrastructure.Identity
{
    public static class ServiceExtensions
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(option =>
                {
                    option.UseInMemoryDatabase("ApplicationDb");
                });
            }
            else
            {
                services.AddDbContext<IdentityContext>(option =>
                {
                    option.UseSqlServer(configuration.GetConnectionString("DealNotifierSecurityConnection"),
                        optionAction => optionAction.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                });
            }

            #endregion DbContext

            #region Configure

            services.Configure<JWTConfig>(configuration.GetSection("JWTConfig"));

            #endregion Configure

            #region Identity

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(Token.Provider);

            #endregion Identity

            #region Dependency injection

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();


            #endregion Dependency injection

            #region Authentication

            var key = Encoding.UTF8.GetBytes(configuration["JWTConfig:key"]);
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = configuration["JWTConfig:Audience"],
                    ValidIssuer = configuration["JWTConfig:Issuer"],
                };
                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = async (context) =>
                    {
                        var statusCode = StatusCodes.Status401Unauthorized;
                        context.Response.StatusCode = statusCode;
                        context.HandleResponse();

                        var response = new ApiResponse<Exception>
                        {
                            StatusCode = statusCode,
                            Message = "You are not authenticated"
                        };

                        await context.Response.WriteAsJsonAsync(response);

                    },
                    OnForbidden = async (context) =>
                    {
                        var statusCode = StatusCodes.Status403Forbidden;
                        context.Response.StatusCode = statusCode;
                        context.Response.ContentType = "application/json";

                        var response = new ApiResponse<Exception>
                        {
                            StatusCode = statusCode,
                            Message = "You are not authorized to access this resource"
                        };

                        await context.Response.WriteAsJsonAsync(response);                    
                    }
                };
            });

            #endregion Authentication
        }
    }
}
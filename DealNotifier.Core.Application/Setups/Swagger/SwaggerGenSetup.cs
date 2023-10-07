using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DealNotifier.Core.Application.Setups.Swagger
{
    public static class SwaggerGenSetup
    {
        public static readonly Action<SwaggerGenOptions> Configure = options =>
        {

            options.OperationFilter<SwaggerDefaultValues>();

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: Bearer 12345abcdef",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = JwtBearerDefaults.AuthenticationScheme
                                    },
                        Scheme = "0auth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

        };
    }
}
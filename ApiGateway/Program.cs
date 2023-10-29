using ApiGateway.Interfaces;
using ApiGateway.Middlewares;
using ApiGateway.Providers;
using ApiGateway.Services;
using ApiGateway.Setups;
using ApiGateway.Setups.Swagger;
using ApiGateway.Wrappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
string identityServiceAddress;



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(SwaggerGenSetup.Configure);
builder.Services.AddCors(CorsSetup.Configure);
builder.Services.AddApiVersioning(ApiVersionSetup.Configure)
    .AddApiExplorer(VersionedApiExplorerSetup.Configure);
builder.Services.AddAuthorization();
builder.Services.AddTransient<GrpcClientInterceptor>();

#region Auth Microservices

identityServiceAddress = configuration["IdentityServiceConfig:Address"] ?? string.Empty;

builder.Services.AddGrpcClient<AuthProto.AuthService.AuthServiceClient>(options =>
{
    options.Address = new Uri(identityServiceAddress);
}).AddInterceptor<GrpcClientInterceptor>();

builder.Services.AddGrpcClient<UserProto.UserService.UserServiceClient>(options =>
{
    options.Address = new Uri(identityServiceAddress);
}).AddInterceptor<GrpcClientInterceptor>();

#endregion Auth Microservices


#region Authentication
RsaSecurityKey securityKey = await PublicKeyProvider.GetIdentityMicroServicePublicKeyAsync(identityServiceAddress);


builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.FromMinutes(1),
        IssuerSigningKey = securityKey,
        ValidateIssuerSigningKey = true,
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

            var response = new ApiResponse(statusCode, "You are not authenticated");
            await context.Response.WriteAsJsonAsync(response);
        },
        OnForbidden = async (context) =>
        {
            var statusCode = StatusCodes.Status403Forbidden;
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse(statusCode, "You are not authorized to access this resource");
            await context.Response.WriteAsJsonAsync(response);
        }
    };
});

#endregion Authentication

#region Dependency Injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
#endregion Dependency Injection



builder.Host.UseSerilog(SeriLogSetup.Configure);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Asp.Versioning.ApiExplorer;
using DealNotifier.API.Middlewares;
using DealNotifier.Core.Application;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Infrastructure.Email;
using DealNotifier.Infrastructure.Identity;
using DealNotifier.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

#region Add services to the container

builder.Services.AddApplicationServices(configuration);
builder.Services.AddPersistenceServices(configuration);
builder.Services.AddIdentityServices(configuration);
builder.Services.AddEmailServices(configuration);

#endregion Add services to the container

builder.Host.UseSerilog(SeriLogSetup.Configure);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
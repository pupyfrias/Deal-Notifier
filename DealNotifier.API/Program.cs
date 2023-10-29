using Asp.Versioning.ApiExplorer;
using Catalog.Application;
using Catalog.Persistence;
using DealNotifier.API.Middlewares;
using DealNotifier.Infrastructure.Identity;
using Email;
using Identity.Application.SetupOptions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

#region Add services to the container

builder.Services.AddApplicationLayer(configuration);
builder.Services.AddPersistenceInfrastructure(configuration);
builder.Services.AddIdentityInfrastructure(configuration);
builder.Services.AddInfrastructureEmail(configuration);

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
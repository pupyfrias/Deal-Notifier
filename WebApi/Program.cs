using DealNotifier.Core.Application;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Infrastructure.Email;
using DealNotifier.Infrastructure.Persistence;
using DealNotifier.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using WebApi.Middlewares;

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
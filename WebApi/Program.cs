using Serilog;
using WebApi.Extensions;
using DealNotifier.Core.Application;
using DealNotifier.Core.Application.SetupOptions;
using DealNotifier.Infrastructure.Identity;
using DealNotifier.Infrastructure.Persistence;
using WebApi.Middlewares;
using DealNotifier.Infrastructure.Email;
{
    var builder = WebApplication.CreateBuilder(args);
    ConfigurationManager configuration = builder.Configuration;

    #region Add services to the container

    builder.Services.AddApplicationLayer(configuration);
    builder.Services.AddPersistenceInfrastructure(configuration);
    builder.Services.AddIdentityInfrastructure(configuration);
    builder.Services.AddInfrastructureEmail(configuration);

    #endregion Add services to the container

    builder.Host.UseSerilog(SeriLog.Options);
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
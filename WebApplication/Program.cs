using Serilog;
using WebApi.Extensions;
using WebScraping.Core.Application;
using WebScraping.Core.Application.SetupOptions;
using WebScraping.Infrastructure.Identity;
using WebScraping.Infrastructure.Persistence;

{
    var builder = WebApplication.CreateBuilder(args);
    ConfigurationManager configuration = builder.Configuration;

    #region Add services to the container

    builder.Services.AddApplicationLayer(configuration);
    builder.Services.AddPersistenceInfrastructure(configuration);
    builder.Services.AddIdentityInfrastructure(configuration);

    #endregion Add services to the container

    builder.Host.UseSerilog(SeriLog.Options);
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.AddMiddlewares();
    app.MapControllers();
    app.Run();
}
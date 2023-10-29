using Identity.Application;
using Identity.Application.SetupOptions;
using Identity.GrpcService.Interceptors;
using Identity.GrpcService.Services;
using Identity.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;



builder.Services.AddApplicationLayer(configuration);


builder.Services.AddGrpc(options=>
{
    options.Interceptors.Add<ErrorHandlingInterceptor>();
});



builder.Services.AddPersistenceInfrastructure(configuration);


builder.Host.UseSerilog(SeriLogSetup.Configure);

var app = builder.Build();



app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
app.MapGrpcService<AuthServiceImpl>();
app.MapGrpcService<UserServiceImpl>();
app.MapGrpcService<RoleServiceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

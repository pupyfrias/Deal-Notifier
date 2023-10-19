using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Core.Domain.Configs;
using DealNotifier.Infrastructure.Email.Service;
using DealNotifier.Infrastructure.Email.Settings;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Infrastructure.Persistence.Repositories;
using DealNotifier.Infrastructure.Persistence.Setup;
using EbayDataSyncWorker;
using Serilog;
using System.Configuration;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

/*if (environment != "Development")
{
    Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}*/

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "eBayDataSync";
    })
    .UseSerilog(SeriLogSetup.Configure)
    .ConfigureServices((hostContext, services) =>
    {
        #region Configure
        services.Configure<EbayUrlConfig>(hostContext.Configuration.GetSection("EbayUrlConfig"));
        services.Configure<MailSettings>(hostContext.Configuration.GetSection("MailSettings"));
        #endregion Configure

        services.AddHostedService<Worker>();
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddHttpClient();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddDbContext<ApplicationDbContext>(DbContextSetup.Configure(hostContext.Configuration), ServiceLifetime.Transient);
        services.AddScoped<IBanKeywordRepository, BanKeywordRepository>();
        services.AddScoped<IBanLinkRepository, BanLinkRepository >();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemSyncService, ItemSyncService>();
        services.AddScoped<INotificationCriteriaRepository, NotificationCriteriaRepository>();
        services.AddScoped<IPhoneCarrierRepository, PhoneCarrierRepository>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierRepository, UnlockabledPhonePhoneCarrierRepository>();
        services.AddScoped<IUnlockabledPhoneRepository, UnlockabledPhoneRepository>();
        services.AddScoped<IItemSyncRepository,ItemSyncRepository>();
        services.AddScoped<IUnlockabledPhoneService, UnlockabledPhoneService>();

    })
   .Build();



await host.RunAsync();
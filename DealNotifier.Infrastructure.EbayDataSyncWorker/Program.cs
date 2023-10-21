using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Services.Items;
using DealNotifier.Core.Application.Setups;
using DealNotifier.Core.Domain.Configs;
using DealNotifier.Infrastructure.EbayDataSyncWorker;
using DealNotifier.Infrastructure.EbayDataSyncWorker.Interfaces;
using DealNotifier.Infrastructure.EbayDataSyncWorker.Services;
using DealNotifier.Infrastructure.Email.Service;
using DealNotifier.Infrastructure.Email.Settings;
using DealNotifier.Persistence.DbContexts;
using DealNotifier.Persistence.Setup;
using DealNotifier.Persistence.Repositories;
using Serilog;
using System.Reflection;

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

if (environment != "Development")
{
    Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}

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

        #region Repositories
        services.AddScoped<IBanKeywordRepository, BanKeywordRepository>();
        services.AddScoped<IBanLinkRepository, BanLinkRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<INotificationCriteriaRepository, NotificationCriteriaRepository>();
        services.AddScoped<IPhoneCarrierRepository, PhoneCarrierRepository>();
        services.AddScoped<IPhoneCarrierService, PhoneCarrierService>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierRepository, UnlockabledPhonePhoneCarrierRepository>();
        services.AddScoped<IUnlockabledPhonePhoneUnlockToolRepository, UnlockabledPhonePhoneUnlockToolRepository>();
        services.AddScoped<IUnlockabledPhoneRepository, UnlockabledPhoneRepository>();
        services.AddScoped<IUnlockProbabilityRepository, UnlockProbabilityRepository>();
        #endregion Repositories


        #region Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEbayDataSynchronizerService, EbayDataSynchronizerService>();
        services.AddScoped<IEbayFetchService, EbayFetchService>();
        services.AddScoped<IHttpService, HttpService>();
        services.AddScoped<IItemSummaryManagerService, ItemSummaryManagerService>();
        services.AddScoped<IUnlockProbabilityService, UnlockProbabilityService>();
        services.AddScoped<IEbayPhoneProcessService, EbayPhoneProcessService>();
        services.AddScoped<IUnlockabledPhonePhoneCarrierService, UnlockabledPhonePhoneCarrierService>();
        services.AddScoped<IUnlockabledPhonePhoneUnlockToolService, UnlockabledPhonePhoneUnlockToolService>();
        services.AddScoped<IUnlockabledPhoneService, UnlockabledPhoneService>();
        services.AddScoped<IItemDependencyLoaderService, ItemDependencyLoaderService>();
        services.AddScoped<IItemManagerService,ItemManagerService>();
        services.AddScoped<IItemNotificationService,ItemNotificationService>();
        services.AddScoped<IItemValidationService,ItemValidationService>();
        services.AddScoped<ICacheDataService, CacheDataService>();
        services.AddScoped<IUnlockVerificationService,UnlockVerificationService>();
        services.AddScoped<IItemService,ItemService>();
        #endregion Services
    })
   .Build();



await host.RunAsync();
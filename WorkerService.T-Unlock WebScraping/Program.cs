using Serilog;
using System.Reflection;
using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Application.Contracts.Services;
using WebScraping.Core.Application.Services;
using WebScraping.Core.Application.SetupOptions;
using WebScraping.Core.Domain.Configs;
using WebScraping.Infrastructure.Persistence.DbContexts;
using WebScraping.Infrastructure.Persistence.Repositories;
using WorkerService.T_Unlock_WebScraping;


Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSingleton(new ApplicationDbContext());
       
        #region Repositories
        services.AddSingleton(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
        services.AddSingleton<IUnlockableRepositoryAsync, UnlockableRepositoryAsync>();
        services.AddSingleton<IPhoneCarrierRepositoryAsync, PhoneCarrierRepositoryAsync>();
        services.AddSingleton<IUnlockableUnlockToolRepositoryAsync, UnlockableUnlockToolRepositoryAsync>();
        services.AddSingleton<IUnlockablePhoneCarrierRepositoryAsync, UnlockablePhoneCarrierRepositoryAsync>();

        #endregion Repositories

        #region Services
        services.AddSingleton(typeof(IGenericServiceAsync<>), typeof(GenericServiceAsync<>));
        services.AddSingleton<IUnlockableServiceAsync, UnlockableServiceAsync>();
        services.AddSingleton<IPhoneCarrierServiceAsync, PhoneCarrierServiceAsync>();
        services.AddSingleton<IUnlockableUnlockToolServiceAsync, UnlockableUnlockToolServiceAsync>();
        services.AddSingleton<IUnlockablePhoneCarrierServiceAsync, UnlockablePhoneCarrierServiceAsync>();

        #endregion Services


        #region Configure

        services.Configure<TUnlockUrlConfig>(hostContext.Configuration.GetSection("TUnlockUrlConfig"));

        #endregion Configure


    })
    .UseSerilog(SeriLog.Options)
    .Build();


await host.RunAsync();
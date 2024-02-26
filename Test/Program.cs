

using AutoMapper;
using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.Mappings;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.Services.Items;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Email.Service;
using DealNotifier.Infrastructure.Email.Settings;
using DealNotifier.Persistence.DbContexts;
using DealNotifier.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;


var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseSqlServer("Server=localhost;Database=DealNotifier2.0;User=pupyfrias;Password=08143611;Max Pool Size=2000;MultipleActiveResultSets=True;Encrypt=False;Connection Timeout=60")
   /* .LogTo(Console.WriteLine, LogLevel.Information)*/;

var context = new ApplicationDbContext(optionsBuilder.Options);

HttpContextAccessor httpContext = new HttpContextAccessor();
var memoryCacheOptions = new MemoryCacheOptions();
IMemoryCache cache = new MemoryCache(memoryCacheOptions);


var mapperConfiguration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<UnlockProbabilityMapperConfig>();
    cfg.AddProfile<PhoneCarrierMapperConfig>();
    cfg.AddProfile<NotificationCriteriaMapperConfig>();
});

var configurationProvider = mapperConfiguration;

IMapper mapper = new Mapper(configurationProvider);

IUnlockabledPhoneRepository unlockabledPhoneRepository = new UnlockabledPhoneRepository(context, configurationProvider);
IUnlockabledPhonePhoneCarrierRepository unlockabledPhonePhoneCarrierRepository = new UnlockabledPhonePhoneCarrierRepository(context);




    
var unlockProbabilityRepository = new UnlockProbabilityRepository(context, configurationProvider);

var cacheDataService = new CacheDataService();
var serilog = new Serilog.LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();


ItemValidationService itemValidationService = new ItemValidationService(cacheDataService);

var configuration = new ConfigurationBuilder().Build();

var services = new ServiceCollection();
services.AddScoped<ICacheDataService, CacheDataService>();
services.AddScoped<IEmailService, EmailService>();
services.AddScoped<IItemValidationService, ItemValidationService>();
services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
services.AddSerilog(serilog);

var serviceProvider = services.BuildServiceProvider();

var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();


UnlockVerificationService unlockVerificationService = new UnlockVerificationService(unlockabledPhoneRepository,unlockabledPhonePhoneCarrierRepository,cacheDataService);

var phoneCarrierRepository = new PhoneCarrierRepository(context, configurationProvider);

var phoneCarrierService = new PhoneCarrierService(phoneCarrierRepository,mapper,httpContext,cache);

var unlockabledPhonePhoneCarrierService = new UnlockabledPhonePhoneCarrierService(unlockabledPhonePhoneCarrierRepository,mapper,serilog,phoneCarrierService);
var unlockabledPhonePhoneUnlockToolRepository = new UnlockabledPhonePhoneUnlockToolRepository(context);
var unlockabledPhonePhoneUnlockToolService = new UnlockabledPhonePhoneUnlockToolService(unlockabledPhonePhoneUnlockToolRepository,mapper);

var unlockabledPhoneService = new UnlockabledPhoneService(unlockabledPhoneRepository,mapper,httpContext,cache,unlockabledPhonePhoneCarrierService,unlockabledPhonePhoneUnlockToolService );
var unlockProbabilityService = new UnlockProbabilityService(unlockProbabilityRepository,mapper,httpContext,cache, itemValidationService,unlockVerificationService);
var itemNotificationService = new ItemNotificationService(cacheDataService,serilog, serviceScopeFactory);
var notificationCriteriaRepository = new NotificationCriteriaRepository(context, configurationProvider);


async Task evaluateUnlockProbability()
{
    var item = new ItemDto
    {
        BrandId = 1,
        Title = "Samsung Galaxy S9+ 64GB Black SM-G965U1 (Unlocked) Damaged See Details MD7849",
        ShortDescription = "Be sure to look through the photos so you can see the condition of this device. This is being sold AS IS. Screen cracked (see photos). Let us help you find your next great thing! ADD TO FAVORITES. Not Included.",
        BidCount = 0,
        ConditionId = 2,
        Image = "https://i.ebayimg.com/images/g/GKEAAOSwooBjF1Gl/s-l1600.jpg",
        IsAuction = false,
        ItemEndDate = null,
        ItemTypeId = 5,
        Link = "https://www.ebay.com/itm/285461586805",
        OnlineStoreId = 5,
        Price = 49.83m,
        StockStatusId = 1,
        UnlockabledPhoneId = null,
        UnlockProbabilityId = 2,
        Notify = false,

    };


    var phoneCarrierList = await phoneCarrierRepository.GetAllProjectedAsync<PhoneCarrierDto>();
    cacheDataService.PhoneCarrierList = phoneCarrierList.ToHashSet();

    await unlockabledPhoneService.TryAssignUnlockabledPhoneIdAsync(item);
    await unlockProbabilityService.SetUnlockProbabilityAsync(item);
}
 async Task EvaluateNotification()
{


    var item = new Item
    {
        BrandId = 1,
        Title = "Samsung Galaxy C5 SM-C5000 4G LTE Unlocked Smart Cell Phone *READ / FOR PARTS",
        ShortDescription = "When we plug up the phone, it just shows the battery logo. It won't power on. LCD is good. Also, the glass has cracks across the top. See photos. GB unknown. We will respond to you within 24 hours and do our best to help you out.",
        BidCount = 0,
        ConditionId = 2,
        Image = "https://i.ebayimg.com/images/g/GKEAAOSwooBjF1Gl/s-l1600.jpg",
        IsAuction = false,
        ItemEndDate = null,
        ItemTypeId = 5,
        Link = "https://www.ebay.com/itm/285461586805",
        OnlineStoreId = 2,
        Price = 34.75m,
        StockStatusId = 1,
        UnlockabledPhoneId = null,
        UnlockProbabilityId = 4,
        Notify = true,
    };

    var notificationCriteria = await notificationCriteriaRepository.GetAllProjectedAsync<NotificationCriteriaDto>();
    cacheDataService.NotificationCriteriaList = notificationCriteria.ToHashSet();


    itemNotificationService.EvaluateIfNotifiable(item);
    Console.WriteLine(item.Notified);

}

await EvaluateNotification();

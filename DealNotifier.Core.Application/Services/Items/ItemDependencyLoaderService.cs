using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using Serilog;


namespace DealNotifier.Core.Application.Services.Items
{
    public class ItemDependencyLoaderService : IItemDependencyLoaderService
    {
        #region Fields

        private readonly IBanKeywordRepository _banKeywordRepository;
        private readonly IBanLinkRepository _banLinkRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ILogger _logger;
        private readonly INotificationCriteriaRepository _notificationCriteriaRepository;
        private readonly IPhoneCarrierRepository _phoneCarrierRepository;
        private readonly ICacheDataService _cacheDataService;

        #endregion Fields

        #region Constructor

        public ItemDependencyLoaderService(
            IBanKeywordRepository banKeywordRepository,
            IBanLinkRepository banLinkRepository,
            IBrandRepository brandRepository,
            ILogger logger,
            IMapper mapper,
            INotificationCriteriaRepository notificationCriteriaRepository,
            IPhoneCarrierRepository phoneCarrierRepository,
            ICacheDataService cacheDataService

            )
        {
            _banKeywordRepository = banKeywordRepository;
            _banLinkRepository = banLinkRepository;
            _brandRepository = brandRepository;
            _logger = logger;
            _notificationCriteriaRepository = notificationCriteriaRepository;
            _phoneCarrierRepository = phoneCarrierRepository;
            _cacheDataService = cacheDataService;

        }

        #endregion Constructor

        #region Methods       

        public async Task LoadDataAsync()
        {
            try
            {
                var tasks = new List<Task>
                {
                    LoadBanKeywordsAsync(),
                    LoadBanLinksAsync(),
                    LoadBrandsAsync(),
                    LoadNotificationCriteriaAsync(),
                    LoadPhoneCarriersAsync()
                };

                await Task.WhenAll(tasks);
                _logger.Information("All data necessary to process the Items was loaded.");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while loading data necessary to process the Items. Exception:{ex.Message}" +
                    $"InnerException: {ex.InnerException?.Message}");
            }
        }

        #region Private Methods

        private async Task LoadBanKeywordsAsync()
        {
            var bandKeywordList = await _banKeywordRepository.GetAllProjectedAsync<BanKeywordDto>();
            _cacheDataService.BanKeywordList = bandKeywordList.ToHashSet();
        }

        private async Task LoadBanLinksAsync()
        {
            var banLinkList = await _banLinkRepository.GetAllProjectedAsync<BanLinkDto>();
            _cacheDataService.BanLinkList = banLinkList.ToHashSet();
        }

        private async Task LoadBrandsAsync()
        {
            var brandList = await _brandRepository.GetAllProjectedAsync<BrandDto>();
            _cacheDataService.BrandList = brandList.ToHashSet();
        }

        private async Task LoadNotificationCriteriaAsync()
        {
            var notificationCriteria = await _notificationCriteriaRepository.GetAllProjectedAsync<NotificationCriteriaDto>();
            _cacheDataService.NotificationCriteriaList = notificationCriteria.ToHashSet();
        }

        private async Task LoadPhoneCarriersAsync()
        {
            var phoneCarrierList = await _phoneCarrierRepository.GetAllProjectedAsync<PhoneCarrierDto>();
            _cacheDataService.PhoneCarrierList = phoneCarrierList.ToHashSet();
        }



        #endregion Private Methods

        #endregion Methods
    }
}
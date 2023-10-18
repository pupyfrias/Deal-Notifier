using AutoMapper;
using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Utilities;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Collections.Concurrent;
using System.Data;
using System.Text.RegularExpressions;


namespace DealNotifier.Core.Application.Services
{
    public class ItemSyncService : IItemSyncService
    {
        #region Fields

       
        private readonly IBanKeywordRepository _banKeywordRepository;
        private readonly IBanLinkRepository _banLinkRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IItemSyncRepository _itemSyncRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly INotificationCriteriaRepository _notificationCriteriaRepository;
        private readonly IPhoneCarrierRepository _phoneCarrierRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IUnlockabledPhonePhoneCarrierRepository _unlockabledPhonePhoneCarrierRepository;
        private readonly IUnlockabledPhoneRepository _unlockabledPhoneRepository;
        private readonly IEmailService _emailService;
        private readonly ConcurrentBag<int> _evaluatedItemIdList = new ConcurrentBag<int>();
        private const  int _maxBidCount = 3;
        private const decimal _minPriceDifferenceForNotification = 5m;
        private const int _maxTimeDifference = 2;


        #endregion Fields

        #region Constructor

        public ItemSyncService(
            IBanKeywordRepository banKeywordRepository,
            IBanLinkRepository banLinkRepository,
            IBrandRepository brandRepository,
            ILogger logger,
            IMapper mapper,
            INotificationCriteriaRepository notificationCriteriaRepository,
            IPhoneCarrierRepository phoneCarrierRepository,
            IUnlockabledPhonePhoneCarrierRepository unlockabledPhonePhoneCarrierRepository,
            IUnlockabledPhoneRepository unlockabledPhoneRepository,
            IServiceScopeFactory serviceScopeFactory,
            IItemSyncRepository itemSyncRepository,
            IEmailService emailService


            )
        {
            _banKeywordRepository = banKeywordRepository;
            _banLinkRepository = banLinkRepository;
            _brandRepository = brandRepository;
            _logger = logger;
            _mapper = mapper;
            _notificationCriteriaRepository = notificationCriteriaRepository;
            _phoneCarrierRepository = phoneCarrierRepository;
            _unlockabledPhonePhoneCarrierRepository = unlockabledPhonePhoneCarrierRepository;
            _unlockabledPhoneRepository = unlockabledPhoneRepository;
            _serviceScopeFactory = serviceScopeFactory;
            _itemSyncRepository = itemSyncRepository;
            _emailService = emailService;
        }

        #endregion Constructor

        #region Methods

        public bool CanBeSaved(ItemCreateRequest itemCreate)
        {
            var isBannedKeyword = CheckBanKeywords(itemCreate);
            var isBannedLink = CheckBanList(itemCreate);
            return !(isBannedKeyword || isBannedLink);
        }

       

        public async Task LoadNecessaryDataAsync()
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
                _logger.Information("All needed data Loaded");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while loading data: {ex.Message}");
            }
        }




        public async Task SaveOrUpdateAsync(ConcurrentBag<ItemCreateRequest> itemCreateList)
        {
            var itemListToCreate = new ConcurrentBag<Item>();
            var itemListToUpdate = new ConcurrentBag<Item>();

            try
            {
                var tasks = itemCreateList.Select(async itemCreate =>
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();
                        var oldItem = await itemRepository.FirstOrDefaultAsync(i => i.Link == itemCreate.Link);
                        
                        if (oldItem == null)
                        {
                            Item newItem = CreateNewItem(itemCreate);
                            itemListToCreate.Add(newItem);
                        }
                        else
                        {
                            UpdateExistingItem(oldItem, itemCreate);
                            itemListToUpdate.Add(oldItem);
                        }
                    }
                });

                await Task.WhenAll(tasks);

                var createTask = CreateRangeAsync(itemListToCreate);
                var updateTask = UpdateRangeAsync(itemListToUpdate);

                await Task.WhenAll(updateTask, createTask);
                itemCreateList.Clear();
                _logger.Information($"{itemListToCreate.Count} items created | {itemListToUpdate.Count} items updated");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }



        public async Task SetUnlockProbabilityAsync(ItemCreateRequest itemCreate)
        {
            if (ContainsUnlockedWord(itemCreate.Name) || await CanBeUnlockedBasedOnUnlockabledPhoneIdAsync(itemCreate))
            {
                itemCreate.UnlockProbabilityId = (int)Enums.UnlockProbability.High;
            }

            else if (await CanBeUnlockedBasedOnModelNameAsync(itemCreate))
            {
                itemCreate.UnlockProbabilityId = (int)Enums.UnlockProbability.Middle;
            }
            else
            {
                itemCreate.UnlockProbabilityId = (int)Enums.UnlockProbability.Low;
            }
        }



        public async Task TryAssignUnlockabledPhoneIdAsync(ItemCreateRequest itemCreate)
        {
            var possibleModelNumber = Regex.Match(itemCreate.Name, RegExPattern.ModelNumber, RegexOptions.IgnoreCase).Value;

            if (!string.IsNullOrEmpty(possibleModelNumber))
            {
                var possibleUnlockabledPhone = await _unlockabledPhoneRepository.FirstOrDefaultAsync(element => element.ModelNumber.Equals(possibleModelNumber));

                if (possibleUnlockabledPhone != null)
                {
                    itemCreate.UnlockabledPhoneId = possibleUnlockabledPhone.Id;
                }
            }
            
        }


        public async Task UpdateStockStatusAsync(Enums.OnlineStore onlineStore)
        {

            string query = "EXEC Update_StockStatus @IdListString, @OnlineStoreId, @OutputResult OUTPUT, @ErrorMessage OUTPUT";

            var idListString = string.Join(',', _evaluatedItemIdList);
            
            var idListStringParameter = new SqlParameter("@IdListString", idListString);
            var onlineStoreIdParameter = new SqlParameter("@OnlineStoreId",(int) onlineStore);
            var outputResultParameter = new SqlParameter("@OutputResult", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            var errorMessageParameter = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };

            await _itemSyncRepository.UpdateStockStatusAsync(query, idListStringParameter, onlineStoreIdParameter, outputResultParameter, errorMessageParameter);

            _logger.Information($"{_evaluatedItemIdList.Count} Items In Stock");
            _evaluatedItemIdList.Clear();
        }



        #region Private Methods


        private bool ContainsUnlockedWord(string title)
        {
            return title.Contains("unlocked", StringComparison.OrdinalIgnoreCase) || title.Contains("ulk", StringComparison.OrdinalIgnoreCase);
        }


        private async Task<bool> CanBeUnlockedBasedOnUnlockabledPhoneIdAsync(ItemCreateRequest itemCreate)
        {
            if (itemCreate.UnlockabledPhoneId != null)
            {
                var carrierId = TryGetPhoneCarrierId(itemCreate.Name);
                if (carrierId != null)
                {
                    return await ExistsUnlockabledPhonePhoneCarrier((int)itemCreate.UnlockabledPhoneId, (int)carrierId);
                }
            }

            return false;
        }

        private async Task<bool> ExistsUnlockabledPhonePhoneCarrier(int unlockabledPhoneId, int carrierId)
        {
            var unlockabledPhonePhoneCarrier = new UnlockabledPhonePhoneCarrier
            {
                PhoneCarrierId = carrierId,
                UnlockabledPhoneId = unlockabledPhoneId
            };

            return await _unlockabledPhonePhoneCarrierRepository.ExistsAsync(unlockabledPhonePhoneCarrier);
        }


        private async Task<bool> CanBeUnlockedBasedOnModelNameAsync(ItemCreateRequest itemCreate)
        {
            var possibleModelName = Regex.Match(itemCreate.Name, RegExPattern.ModelName, RegexOptions.IgnoreCase).Value;

            if (possibleModelName != null)
            {
                var possibleUnlockedPhone = await _unlockabledPhoneRepository.FirstOrDefaultAsync(item => item.ModelName!.Contains(possibleModelName));

                if (possibleUnlockedPhone != null)
                {
                    var carrierId = TryGetPhoneCarrierId(itemCreate.Name);
                    if (carrierId != null)
                    {
                        return await ExistsUnlockabledPhonePhoneCarrier(possibleUnlockedPhone.Id, (int)carrierId);

                    }
                }
            }

            return false;
        }


        private void EvaluateIfNotifiable(Item item)
        {
            DateTime notified = item.Notified.HasValue ? (DateTime)item.Notified : DateTime.MinValue;
            DateTime itemEndDate = item.ItemEndDate.HasValue ? (DateTime)item.ItemEndDate : DateTime.MinValue;

            foreach (NotificationCriteriaDto notificationCriteria in CacheDataUtility.NotificationCriteriaList)
            {
                if (notificationCriteria.ConditionId == item.ConditionId
                    && notificationCriteria.MaxPrice >= item.Price
                    && TitleContainsKeyword(item.Name, notificationCriteria.Keywords)
                    && (bool)item.Notify!
                    && notified.Date != DateTime.Now.Date

                    )
                {
                    if (item.ItemTypeId == (int)Enums.ItemType.Phone && item.UnlockProbabilityId == (int)Enums.UnlockProbability.Low)
                    {
                        break;
                    }

                    if ((bool)item.IsAuction!)
                    {

                        var timeDifference = (itemEndDate - DateTime.Now).TotalHours;
                        if (timeDifference > _maxTimeDifference || timeDifference < 0 || item.BidCount > _maxBidCount)
                        {
                            break;
                        }
                    }

                    _emailService.NotifiableItemList.Add(item);
                    item.Notified = DateTime.Now;
                    break;
                }
            }
        }


        private Item CreateNewItem(ItemCreateRequest itemCreate)
        {
            var mappedItem = _mapper.Map<Item>(itemCreate);
            EvaluateIfNotifiable(mappedItem);
            return mappedItem;
        }


        private void UpdateExistingItem(Item oldItem, ItemCreateRequest item)
        {
            _evaluatedItemIdList.Add(oldItem.Id);
            decimal oldPrice = oldItem.Price;
            decimal saving = oldPrice - item.Price;

            if (oldPrice != item.Price ||
                oldItem.UnlockProbabilityId != item.UnlockProbabilityId ||
                oldItem.Image != item.Image ||
                item.IsAuction
                )
            {
                if (oldPrice > item.Price)
                {
                    oldItem.Saving = saving;
                    oldItem.SavingsPercentage = saving / oldPrice * 100;
                    oldItem.OldPrice = oldPrice;
                }
                else if (oldPrice < item.Price)
                {
                    oldItem.OldPrice = 0;
                    oldItem.Saving = 0;
                    oldItem.SavingsPercentage = 0;
                }

                oldItem.Notified = null;
                _mapper.Map(item, oldItem);
                if ((oldPrice - item.Price) >= _minPriceDifferenceForNotification || item.IsAuction)
                {
                    EvaluateIfNotifiable(oldItem);
                }
            }
        }


        private async Task CreateRangeAsync(ConcurrentBag<Item> itemListToCreate)
        {
            if (itemListToCreate.Count > 0)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();
                    await itemRepository.CreateRangeAsync(itemListToCreate);
                    foreach (var item in itemListToCreate)
                    {
                        _evaluatedItemIdList.Add(item.Id);
                    }
                }
            }
        }

        private async Task UpdateRangeAsync(ConcurrentBag<Item> itemListToUpdate)
        {

            if (itemListToUpdate.Count > 0)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();
                    await itemRepository.UpdateRangeAsync(itemListToUpdate);
                }
            }
        }

        private async Task LoadBanKeywordsAsync()
        {
            var bandKeywordList = await _banKeywordRepository.GetAllProjectedAsync<BanKeywordDto>();
            CacheDataUtility.BanKeywordList = bandKeywordList.ToHashSet<BanKeywordDto>();
        }

        private async Task LoadBanLinksAsync()
        {
            var banLinkList = await _banLinkRepository.GetAllProjectedAsync<BanLinkDto>();
            CacheDataUtility.BanLinkList = banLinkList.ToHashSet<BanLinkDto>();
        }

        private async Task LoadBrandsAsync()
        {
            var brandList = await _brandRepository.GetAllProjectedAsync<BrandDto>();
            CacheDataUtility.BrandList = brandList.ToHashSet<BrandDto>();
        }

        private async Task LoadNotificationCriteriaAsync()
        {
            var notificationCriteria = await _notificationCriteriaRepository.GetAllProjectedAsync<NotificationCriteriaDto>();
            CacheDataUtility.NotificationCriteriaList = notificationCriteria.ToHashSet<NotificationCriteriaDto>();
        }

        private async Task LoadPhoneCarriersAsync()
        {
            var phoneCarrierList = await _phoneCarrierRepository.GetAllProjectedAsync<PhoneCarrierDto>();
            CacheDataUtility.PhoneCarrierList = phoneCarrierList.ToHashSet<PhoneCarrierDto>();
        }

        private bool TitleContainsKeyword(string title, string keywords)
        {
            var keywordList = keywords.Split(',').Select(keyword => keyword.Trim());
            return keywordList.Any(keyword => title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        private int? TryGetPhoneCarrierId(string name)
        {
            var matchedPhoneCarrier = CacheDataUtility.PhoneCarrierList.FirstOrDefault(carrier =>
            {
                var PhoneCarrierNameAndShortNameList = carrier.Name.Split('|').ToList();

                return PhoneCarrierNameAndShortNameList.Exists(element => name.Contains(element, StringComparison.OrdinalIgnoreCase));
            });

            return matchedPhoneCarrier?.Id ?? null;
        }
        private bool CheckBanKeywords(ItemCreateRequest itemCreate)
        {
            return CacheDataUtility.BanKeywordList.Any(element => itemCreate.Name.Contains(element.Keyword, StringComparison.OrdinalIgnoreCase));
        }

        private bool CheckBanList(ItemCreateRequest itemCreate)
        {
            return CacheDataUtility.BanLinkList.Any(x => x.Link == itemCreate.Link);
        }


        #endregion Private Methods

        #endregion Methods
    }
}
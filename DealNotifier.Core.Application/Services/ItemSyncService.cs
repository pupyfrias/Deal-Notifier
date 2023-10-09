using AutoMapper;
using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Heplers;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Utilities;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace DealNotifier.Core.Application.Services
{
    public class ItemSyncService :IItemSyncService
    {
        #region Fields

        private readonly IEmailService _emailServiceAsync;
        private readonly IItemRepository _itemRepositoryAsync;
        private readonly ILogger _logger;
        private readonly IBanKeywordRepository _banKeywordRepositoryAsync;
        private readonly IBanLinkRepository _banLinkRepositoryAsync;
        private readonly IBrandRepository _brandRepositoryAsync;
        private readonly INotificationCriteriaRepository _notificationCriteriaRepositoryAsync;
        private readonly IPhoneCarrierRepository _phoneCarrierRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IUnlockabledPhonePhoneCarrierRepository _unlockabledPhonePhoneCarrierRepositoryAsync;
        private readonly IUnlockabledPhoneRepository _unlockabledPhoneRepositoryAsync;
        public ConcurrentBag<Item> itemToNotifyList { get; set; } = new ConcurrentBag<Item>();

        #endregion Fields

        #region Constructor

        public ItemSyncService(
            IItemRepository itemRepository,
            IConfigurationProvider configurationProvider,
            IEmailService emailService,
            ILogger logger,
            IBanKeywordRepository banKeywordRepositoryAsync,
            IBanLinkRepository banLinkRepositoryAsync,
            IBrandRepository brandRepositoryAsync,
            INotificationCriteriaRepository notificationCriteriaRepositoryAsync,
            IPhoneCarrierRepository phoneCarrierRepositoryAsync,
            IMapper mapper,
            IUnlockabledPhonePhoneCarrierRepository unlockabledPhonePhoneCarrierRepositoryAsync,
            IUnlockabledPhoneRepository unlockabledPhoneRepositoryAsync

            )
        {
            _logger = logger;
            _emailServiceAsync = emailService;
            _itemRepositoryAsync = itemRepository;
            _banKeywordRepositoryAsync = banKeywordRepositoryAsync;
            _banLinkRepositoryAsync = banLinkRepositoryAsync;
            _brandRepositoryAsync = brandRepositoryAsync;
            _notificationCriteriaRepositoryAsync = notificationCriteriaRepositoryAsync;
            _phoneCarrierRepositoryAsync = phoneCarrierRepositoryAsync;
            _mapper = mapper;
            _unlockabledPhonePhoneCarrierRepositoryAsync = unlockabledPhonePhoneCarrierRepositoryAsync;
            _unlockabledPhoneRepositoryAsync = unlockabledPhoneRepositoryAsync;
        }

        #endregion Constructor

        #region Methods

        public async Task LoadData()
        {
            try
            {
                var tasks = new List<Task>
                {
                    LoadBandKeywords(),
                    LoadBanLinks(),
                    LoadBrands(),
                    LoadNotificationCriteria(),
                    LoadPhoneCarriers()
                };

                await Task.WhenAll(tasks);
                _logger.Information("All needed data Loaded");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while loading data: {ex.Message}");
            }
        }


        public async Task SaveOrUpdate( ConcurrentBag<ItemCreateRequest> items)
        {
            var itemListToCreate = new ConcurrentBag<Item>();
            var itemListToUpdate = new ConcurrentBag<Item>();

            try
            {
                var tasks = items.Select(async item =>
                {
                    var oldItem = await _itemRepositoryAsync.FirstOrDefaultAsync(i=> i.Link == item.Link);
                    if (oldItem == null)
                    {
                        var mappedItem = _mapper.Map<Item>(item);

                        ToNotify(mappedItem);
                        itemListToCreate.Add(mappedItem);
                    }
                    else
                    {
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
                            itemListToUpdate.Add(oldItem);
                            if ((oldPrice - item.Price) >= 5 || item.IsAuction)
                            {
                                ToNotify(oldItem);
                            }
                        }
                    }
                });

                await Task.WhenAll(tasks);

                var createTask = _itemRepositoryAsync.CreateRangeAsync(itemListToCreate);
                var updateTask = _itemRepositoryAsync.UpdateRangeAsync(itemListToUpdate);
                Task.WhenAll(updateTask, createTask).Wait();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }







        public async Task<Enums.UnlockProbability> SetUnlockProbability(ItemCreateRequest item)
        {
            var modelNumber = item.ModelNumber;
            var modelName = item.ModelName;
            var carrierId = item.PhoneCarrierId;

            if (item.Name.Contains("unlocked", StringComparison.OrdinalIgnoreCase)
                || item.Name.Contains("ulk", StringComparison.OrdinalIgnoreCase))
            {
                return Enums.UnlockProbability.High;
            }
            else
            { 
                if (modelNumber != null)
                {

                    var possibleUnlockedPhone = await _unlockabledPhoneRepositoryAsync.FirstOrDefaultAsync(item => item.ModelNumber.Equals(modelNumber));

                    if (possibleUnlockedPhone != null)
                    {
                        var unlockabledPhonePhoneCarrier = new UnlockabledPhonePhoneCarrier
                        {
                            PhoneCarrierId = carrierId,
                            UnlockabledPhoneId = possibleUnlockedPhone.Id
                        };

                        var canBeUnlock = await _unlockabledPhonePhoneCarrierRepositoryAsync.ExistsAsync(unlockabledPhonePhoneCarrier);
                        if(canBeUnlock) return Enums.UnlockProbability.High;
                    }
                }

                if (modelName != null)
                {
                    var possibleUnlockedPhone = await _unlockabledPhoneRepositoryAsync.FirstOrDefaultAsync(item => item.ModelName!.Contains(modelName));

                    if (possibleUnlockedPhone != null)
                    {
                        var unlockabledPhonePhoneCarrier = new UnlockabledPhonePhoneCarrier
                        {
                            PhoneCarrierId = carrierId,
                            UnlockabledPhoneId = possibleUnlockedPhone.Id
                        };

                        var canBeUnlock = await _unlockabledPhonePhoneCarrierRepositoryAsync.ExistsAsync(unlockabledPhonePhoneCarrier);
                        if (canBeUnlock) return Enums.UnlockProbability.High;
                    }
                }

                return Enums.UnlockProbability.Low;
                
            }
        }

        public void TrySetModelNumberModelNameAndBrand(ref ItemCreateRequest item)
        {

            var possibleModelNumber = Regex.Match(item.Name, RegExPattern.ModelNumber, RegexOptions.IgnoreCase).Value;

            if (!String.IsNullOrEmpty(possibleModelNumber))
            {
                var unlockabledPhone = _unlockabledPhoneRepositoryAsync.FirstOrDefaultAsync(u => u.ModelNumber.Equals(possibleModelNumber));

                if (unlockabledPhone != null)
                {
                    item.BrandId = unlockabledPhone.Id;
                    item.ModelName = unlockabledPhone.ModelName;
                    item.ModelNumber = unlockabledPhone.ModelNumber;
                }
            }

            if (item.BrandId == null && item.ModelName == null && item.ModelNumber == null)
            {
                #region set brand

                foreach (var brand in CacheDataUtility.BrandList)
                {
                    bool isMatched = item.Name.IndexOf(brand.Name, StringComparison.OrdinalIgnoreCase) > -1;
                    if (isMatched)
                    {
                        item.BrandId = brand.Id;
                        break;
                    }
                }

                #endregion set brand

                #region set modelName & phone Carrier

                UnlockableReadDto? unlockable;
                var possibleModelName = Regex.Match(
                item.Name,
                "((?<=samsung\\s+)(galaxy\\s+)?\\w+[+]?)|((?<=motorola\\s+)moto\\s+\\w+)|(lg\\s+\\w+( \\d)?)",
                RegexOptions.IgnoreCase
                );

                if (possibleModelName.Value != string.Empty)
                {
                    int phoneCarrierId = item.PhoneCarrierId ?? (int)Enums.PhoneCarrier.UNK;
                    unlockable = context.UnlockablePhones
                        .Include(x => x.UnlockablePhoneCarriers)
                        .Where(x => x.ModelName.Contains(possibleModelName.Value) &&
                        x.UnlockablePhoneCarriers.Any(u => u.PhoneCarrierId == phoneCarrierId))
                        .ProjectTo<UnlockableReadDto>(_configurationProvider)
                        .FirstOrDefault();

                    if (unlockable != null)
                    {
                        item.ModelName = unlockable.ModelName;
                        item.ModelNumber = unlockable.ModelNumber;
                    }
                    else
                    {
                        unlockable = context.UnlockablePhones
                        .Where(x => x.ModelName.Contains(possibleModelName.Value))
                        .ProjectTo<UnlockableReadDto>(_configurationProvider)
                        .FirstOrDefault();

                        if (unlockable != null)
                        {
                            item.ModelName = unlockable.ModelName;
                        }
                    }

                    #endregion set modelName & phone Carrier
                }
            }
            
        }

        public void UpdateStatus(ref ConcurrentBag<string> checkedList)
        {
            /*using (var context = new ApplicationDbContext())
            {
                var listId = checkedList.Select(x => x.Replace("https://www.ebay.com/itm/", ""));
                string query = "EXEC UPDATE_STATUS_EBAY @ListId, @OutputResult OUTPUT";
                var listIdParameter = new SqlParameter("@ListId", string.Join(',', listId));
                var outputResult = new SqlParameter("@OutputResult", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                context.Database.ExecuteSqlRaw(query, listIdParameter, outputResult);
                bool result = (bool)outputResult.Value;
                _logger.Information($"El resultado es: {result}");
            }

            _logger.Information($"{checkedList.Count} Items In Stock");
            checkedList.Clear();*/
        }

        private void ToNotify(Item item)
        {
            DateTime notified = item.Notified.HasValue ? (DateTime)item.Notified : DateTime.MinValue;
            DateTime itemEndDate = item.ItemEndDate.HasValue ? (DateTime)item.ItemEndDate : DateTime.MinValue;

            foreach (NotificationCriteriaDto conditionsToNotify in CacheDataUtility.NotificationCriteriaList)
            {
                if (conditionsToNotify.ConditionId == item.ConditionId
                    && conditionsToNotify.MaxPrice >= item.Price
                    && CheckKeywords(conditionsToNotify.Keywords, item.Name)
                    && item.Notify
                    && notified.Date != DateTime.Now.Date

                    )
                {
                    if (item.TypeId == (int)Enums.ItemType.Phone && item.UnlockProbabilityId == (int)Enums.UnlockProbability.Low)
                    {
                        break;
                    }

                    if (item.IsAuction)
                    {
                        var diff = (itemEndDate - DateTime.Now).TotalHours;
                        if (diff > 2 || diff < 0 || item.BidCount > 3)
                        {
                            break;
                        }
                    }

                    itemToNotifyList.Add(item);
                    item.Notified = DateTime.Now;
                    break;
                }
            }
        }

        #region Private Methods

        private bool CheckKeywords(string keywords, string title)
        {
            var keywordList = keywords.Split(',');
            return keywordList.Any(keyword => title.IndexOf(keyword) > -1);
        }

        private async Task LoadBandKeywords()
        {
            var bandKeywordList = await _banKeywordRepositoryAsync.GetAllProjectedAsync<BanKeywordDto>();
            CacheDataUtility.BanKeywordList = bandKeywordList.ToHashSet<BanKeywordDto>();
        }

        private async Task LoadBanLinks()
        {
            var banLinkList = await _banLinkRepositoryAsync.GetAllProjectedAsync<BanLinkDto>();
            CacheDataUtility.BanLinkList = banLinkList.ToHashSet<BanLinkDto>();
        }

        private async Task LoadBrands()
        {
            var brandList = await _brandRepositoryAsync.GetAllProjectedAsync<BrandDto>();
            CacheDataUtility.BrandList = brandList.ToHashSet<BrandDto>();
        }

        private async Task LoadNotificationCriteria()
        {
            var notificationCriteria = await _notificationCriteriaRepositoryAsync.GetAllProjectedAsync<NotificationCriteriaDto>();
            CacheDataUtility.NotificationCriteriaList = notificationCriteria.ToHashSet<NotificationCriteriaDto>();
        }

        private async Task LoadPhoneCarriers()
        {
            var phoneCarrierList = await _phoneCarrierRepositoryAsync.GetAllProjectedAsync<PhoneCarrierDto>();
            CacheDataUtility.PhoneCarrierList = phoneCarrierList.ToHashSet<PhoneCarrierDto>();
        }




        #endregion Private Methods

        #endregion Methods
    }
}
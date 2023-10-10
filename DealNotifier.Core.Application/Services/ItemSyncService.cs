using AutoMapper;
using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Utilities;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;
using Serilog;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace DealNotifier.Core.Application.Services
{
    public class ItemSyncService :IItemSyncService
    {
        #region Fields

        private readonly IEmailService _emailService;
        private readonly IItemRepository _itemRepository;
        private readonly ILogger _logger;
        private readonly IBanKeywordRepository _banKeywordRepository;
        private readonly IBanLinkRepository _banLinkRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly INotificationCriteriaRepository _notificationCriteriaRepository;
        private readonly IPhoneCarrierRepository _phoneCarrierRepository;
        private readonly IMapper _mapper;
        private readonly IUnlockabledPhonePhoneCarrierRepository _unlockabledPhonePhoneCarrierRepository;
        private readonly IUnlockabledPhoneRepository _unlockabledPhoneRepository;
        public ConcurrentBag<Item> itemToNotifyList { get; set; } = new ConcurrentBag<Item>();

        #endregion Fields

        #region Constructor

        public ItemSyncService(
            IItemRepository itemRepository,
            IConfigurationProvider configurationProvider,
            IEmailService emailService,
            ILogger logger,
            IBanKeywordRepository banKeywordRepository,
            IBanLinkRepository banLinkRepository,
            IBrandRepository brandRepository,
            INotificationCriteriaRepository notificationCriteriaRepository,
            IPhoneCarrierRepository phoneCarrierRepository,
            IMapper mapper,
            IUnlockabledPhonePhoneCarrierRepository unlockabledPhonePhoneCarrierRepository,
            IUnlockabledPhoneRepository unlockabledPhoneRepository

            )
        {
            _logger = logger;
            _emailService = emailService;
            _itemRepository = itemRepository;
            _banKeywordRepository = banKeywordRepository;
            _banLinkRepository = banLinkRepository;
            _brandRepository = brandRepository;
            _notificationCriteriaRepository = notificationCriteriaRepository;
            _phoneCarrierRepository = phoneCarrierRepository;
            _mapper = mapper;
            _unlockabledPhonePhoneCarrierRepository = unlockabledPhonePhoneCarrierRepository;
            _unlockabledPhoneRepository = unlockabledPhoneRepository;
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
                    var oldItem = await _itemRepository.FirstOrDefaultAsync(i=> i.Link == item.Link);
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

                var createTask = _itemRepository.CreateRangeAsync(itemListToCreate);
                var updateTask = _itemRepository.UpdateRangeAsync(itemListToUpdate);
                Task.WhenAll(updateTask, createTask).Wait();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }







        public void SetUnlockProbability(ref ItemCreateRequest itemCreate)
        {
            var modelNumber = itemCreate.ModelNumber;
            var modelName = itemCreate.ModelName;
            var carrierId = itemCreate.PhoneCarrierId ?? default;

            if (itemCreate.Name.Contains("unlocked", StringComparison.OrdinalIgnoreCase)
                || itemCreate.Name.Contains("ulk", StringComparison.OrdinalIgnoreCase))
            {
               itemCreate.UnlockProbabilityId = (int) Enums.UnlockProbability.High;

            }
            else
            { 
                if (modelNumber != null)
                {

                    var possibleUnlockedPhone =  _unlockabledPhoneRepository.FirstOrDefault(item => item.ModelNumber.Equals(modelNumber));

                    if (possibleUnlockedPhone != null)
                    {
                        var unlockabledPhonePhoneCarrier = new UnlockabledPhonePhoneCarrier
                        {
                            PhoneCarrierId = carrierId,
                            UnlockabledPhoneId = possibleUnlockedPhone.Id
                        };

                        var canBeUnlock = _unlockabledPhonePhoneCarrierRepository.Exists(unlockabledPhonePhoneCarrier);
                        if (canBeUnlock) 
                        {
                            itemCreate.UnlockProbabilityId = (int)Enums.UnlockProbability.High;
                            return;
                        }
                    }
                }

                if (modelName != null)
                {
                    var possibleUnlockedPhone =  _unlockabledPhoneRepository.FirstOrDefault(item => item.ModelName!.Contains(modelName));

                    if (possibleUnlockedPhone != null)
                    {
                        var unlockabledPhonePhoneCarrier = new UnlockabledPhonePhoneCarrier
                        {
                            PhoneCarrierId = carrierId,
                            UnlockabledPhoneId = possibleUnlockedPhone.Id
                        };

                        var canBeUnlock =  _unlockabledPhonePhoneCarrierRepository.Exists(unlockabledPhonePhoneCarrier);
                        if (canBeUnlock)
                        {
                            itemCreate.UnlockProbabilityId = (int)Enums.UnlockProbability.Middle;
                            return;
                        }
                    }
                }

                itemCreate.UnlockProbabilityId = (int)Enums.UnlockProbability.Middle;
            }
        }


        public void TryAssignBrand(ItemCreateRequest itemCreate)
        {
            var matchedBrand = CacheDataUtility.BrandList
                .FirstOrDefault(brand => itemCreate.Name.Contains(brand.Name, StringComparison.OrdinalIgnoreCase));

            if (matchedBrand != null)
            {
                itemCreate.BrandId = matchedBrand.Id;
            }
        }


        /// <summary>
        /// Try to assign values to fields BrandId, ModelName & ModelNumber
        /// </summary>
        /// <param name="itemCreate"></param>
        public void TryAssignItemDetails(ItemCreateRequest itemCreate)
        {
            var possibleModelNumber = Regex.Match(itemCreate.Name, RegExPattern.ModelNumber, RegexOptions.IgnoreCase).Value;

            if (!string.IsNullOrEmpty(possibleModelNumber))
            {
                var possibleUnlockabledPhone = _unlockabledPhoneRepository.FirstOrDefault(element => element.ModelNumber.Equals(possibleModelNumber));

                if (possibleUnlockabledPhone != null)
                {
                    itemCreate.BrandId = possibleUnlockabledPhone.Id;
                    itemCreate.ModelName = possibleUnlockabledPhone.ModelName;
                    itemCreate.ModelNumber = possibleUnlockabledPhone.ModelNumber;
                }
            }
        }

        public void TryAssignModelName(ref ItemCreateRequest itemCreate)
        {

            var possibleModelName = Regex.Match(itemCreate.Name, RegExPattern.ModelNumber,RegexOptions.IgnoreCase).Value;

            if (!string.IsNullOrEmpty(possibleModelName))
            {
                

                var possibleUnlockabledPhone = _unlockabledPhoneRepository.FirstOrDefault(element => element.ModelNumber.Contains(possibleModelName));

                if (possibleUnlockabledPhone != null)
                {
                    itemCreate.ModelName = possibleUnlockabledPhone.ModelName;
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
                    && TitleContainsKeyword(conditionsToNotify.Keywords, item.Name)
                    && item.Notify
                    && notified.Date != DateTime.Now.Date

                    )
                {
                    if (item.ItemTypeId == (int)Enums.ItemType.Phone && item.UnlockProbabilityId == (int)Enums.UnlockProbability.Low)
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

        private bool TitleContainsKeyword(string title, string keywords)
        {
            var keywordList = keywords.Split(',').Select(keyword=> keyword.Trim());
            return keywordList.Any(keyword => title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        private async Task LoadBandKeywords()
        {
            var bandKeywordList = await _banKeywordRepository.GetAllProjectedAsync<BanKeywordDto>();
            CacheDataUtility.BanKeywordList = bandKeywordList.ToHashSet<BanKeywordDto>();
        }

        private async Task LoadBanLinks()
        {
            var banLinkList = await _banLinkRepository.GetAllProjectedAsync<BanLinkDto>();
            CacheDataUtility.BanLinkList = banLinkList.ToHashSet<BanLinkDto>();
        }

        private async Task LoadBrands()
        {
            var brandList = await _brandRepository.GetAllProjectedAsync<BrandDto>();
            CacheDataUtility.BrandList = brandList.ToHashSet<BrandDto>();
        }

        private async Task LoadNotificationCriteria()
        {
            var notificationCriteria = await _notificationCriteriaRepository.GetAllProjectedAsync<NotificationCriteriaDto>();
            CacheDataUtility.NotificationCriteriaList = notificationCriteria.ToHashSet<NotificationCriteriaDto>();
        }

        private async Task LoadPhoneCarriers()
        {
            var phoneCarrierList = await _phoneCarrierRepository.GetAllProjectedAsync<PhoneCarrierDto>();
            CacheDataUtility.PhoneCarrierList = phoneCarrierList.ToHashSet<PhoneCarrierDto>();
        }

        public void TrySetModelNumberModelNameAndBrand(ref ItemCreateRequest itemCreate)
        {
            throw new NotImplementedException();
        }




        #endregion Private Methods

        #endregion Methods
    }
}
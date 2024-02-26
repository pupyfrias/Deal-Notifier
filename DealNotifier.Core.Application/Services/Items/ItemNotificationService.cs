 using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.Email;
using DealNotifier.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Collections.Concurrent;
using System.Text;

namespace DealNotifier.Core.Application.Services.Items
{
    public class ItemNotificationService : IItemNotificationService
    {
        private const int _maxBidCount = 5;
        private const int _maxTimeDifference = 1;
        private readonly ICacheDataService _cacheDataService;
        private readonly IEmailService _emailService;
        private readonly IItemValidationService _itemValidationService;
        private readonly ILogger _logger;
        private readonly ConcurrentBag<Item> _notifiableItems = new();
        public ItemNotificationService(
            ICacheDataService cacheDataService,
            ILogger logger,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _cacheDataService = cacheDataService;
            _itemValidationService  = serviceScopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<IItemValidationService>();
            _logger = logger;
            _emailService =  serviceScopeFactory.CreateScope()
                .ServiceProvider.GetRequiredService<IEmailService>();
        }

        public void EvaluateIfNotifiable(Item item)
        {


            if (!IsItemEligibleForNotification(item) ||
                IsPhoneWithLowUnlockProbability(item) || 
                IsInvalidAuctionItem(item)) return;

            foreach (var notificationCriteria in _cacheDataService.NotificationCriteriaList)
            {
                if (!MatchesCriteria(item, notificationCriteria)) continue;
                NotifyItem(item);
                break;
            }
        }

        public async Task NotifyUsersOfItemsByEmail()
        {
            if (_notifiableItems.Any())
            {
                StringBuilder stringBuilder = new StringBuilder();

                var orderItemToNotifyList = _notifiableItems.OrderBy(item => item.Price);
                _logger.Information($"{orderItemToNotifyList.Count()} Items to Notify");

                foreach (var item in orderItemToNotifyList)
                {
                    var probability = Enum.GetValues<Enums.UnlockProbability>().First(e => (int)e == item.UnlockProbabilityId);
                    var condition = Enum.GetValues<Enums.Condition>().First(e => (int)e == item.ConditionId);

                    stringBuilder.AppendFormat(@$"<div style="" margin: 1rem; background-color: #fff; border-bottom: 2px solid rgba(0, 0, 0, 0.125); background-color: #fff; border-radius: 1rem; padding: 1rem"">
                                                    <div style=""margin: 1rem"">
                                                      <h2 style=""margin: 0"">{item.Title}</h2>
                                                      <p style=""font-size: large; margin: 0""><strong>US$</strong>{item.Price} {(item.OldPrice > 0 ? $"<del style=\"font-size: small\">{item.OldPrice}</del></p>" : "")}
                                                      <p style=""font-size: large; margin: 0""><strong>Unlock Probability: </strong>{probability}</p>
                                                      <p style=""font-size: large; margin: 0""><strong>Condition: </strong>{condition}</p>
                                                      {((bool)item?.IsAuction ? "<p style='font-size: large; margin: 0'><strong>Auction </strong></p>":"")}
                                                    </div>

                                                    <div style=""text-align: center; margin-top: 15px"">
                                                      <a
                                                        href=""{item.Link}""
                                                        style=""display: block""
                                                      >
                                                        <img
                                                          src=""{item.Image}""
                                                          style=""width: 90%; max-height: 535px; object-fit: contain;""
                                                        />
                                                      </a>
                                                      <a
                                                        href=""https://10.0.0.3:8081/api/Items/{item.PublicId}/cancel-notification""
                                                        style=""display: block""
                                                      >
                                                        <p style=""font-size: large; margin: 0"">Cancel Notification</p>
                                                      </a>
                                                    </div>
                                                  </div>");
                }
                string body = @$"<div style=""margin: auto; width: 100%; min-width: 360px; max-width: 1000px; padding: 0.5rem; background-color: #f2f2f2"">
                                  {stringBuilder}
                                </div>"; 

                var email = new EmailDto
                {
                    To = "pupyfrias@gmail.com",
                    Subject = "Item Deals",
                    Body = body
                };

                await _emailService.SendEmailAsync(email);
                _notifiableItems.Clear();
            }
            else
            {
                _logger.Information("There is not items to Notify");
            }
        }

        private bool IsInvalidAuctionItem(Item item)
        {
            if (item.IsAuction == true)
            {
                var timeDifference = (item.ItemEndDate.GetValueOrDefault(DateTime.MinValue) - DateTime.Now).TotalHours;
                return timeDifference > _maxTimeDifference || timeDifference < 0 || item.BidCount > _maxBidCount;
            }
            return false;
        }

        private bool IsItemEligibleForNotification(Item item)
        {
            return item.Notified.GetValueOrDefault(DateTime.MinValue).Date != DateTime.Now.Date && item.Notify == true;
        }

        private bool IsPhoneWithLowUnlockProbability(Item item)
        {
            return item.ItemTypeId == (int)Enums.ItemType.Phone &&
                   item.UnlockProbabilityId == (int)Enums.UnlockProbability.Low;
        }

        private bool MatchesCriteria(Item item, NotificationCriteriaDto criteria)
        {
            var description = $"{item.Title}. {item.ShortDescription}";

            return criteria.ConditionId == item.ConditionId
                   && criteria.MaxPrice >= item.Price
                   && _itemValidationService.DescriptionMatchesIncludeExcludeCriteria(description, criteria.IncludeKeywords, criteria.ExcludeKeywords);
        }
        private void NotifyItem(Item item)
        {
            _notifiableItems.Add(item);
            item.Notified = DateTime.Now;
        }
    }
}

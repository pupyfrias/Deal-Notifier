using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.Email;
using DealNotifier.Core.Domain.Entities;
using Serilog;
using System.Collections.Concurrent;
using System.Text;

namespace DealNotifier.Core.Application.Services.Items
{
    public class ItemNotificationService : IItemNotificationService
    {
        private readonly ICacheDataService _cacheDataService;
        private readonly IItemValidationService _itemValidationService;
        private readonly ILogger _logger;
        private readonly ConcurrentBag<Item> _notifiableItemList = new();
        private const int _maxBidCount = 3;
        private const int _maxTimeDifference = 2;
        private readonly IEmailService _emailService;

        public ItemNotificationService(
            ICacheDataService cacheDataService,
            IItemValidationService itemValidationService,
            ILogger logger,
            IEmailService emailService
            )
        {
            _cacheDataService = cacheDataService;
            _itemValidationService = itemValidationService;
            _logger = logger;
            _emailService = emailService;
        }

        public void EvaluateIfNotifiable(Item item)
        {
            DateTime notified = item.Notified.HasValue ? (DateTime)item.Notified : DateTime.MinValue;
            DateTime itemEndDate = item.ItemEndDate.HasValue ? (DateTime)item.ItemEndDate : DateTime.MinValue;

            foreach (NotificationCriteriaDto notificationCriteria in _cacheDataService.NotificationCriteriaList)
            {
                if (notificationCriteria.ConditionId == item.ConditionId
                    && notificationCriteria.MaxPrice >= item.Price
                    && _itemValidationService.TitleContainsKeyword(item.Name, notificationCriteria.Keywords)
                    && (bool)item.Notify!
                    && notified.Date != DateTime.Now.Date

                    )
                {
                    if (item.ItemTypeId == (int)Enums.ItemType.Phone &&
                        item.UnlockProbabilityId == (int)Enums.UnlockProbability.Low)
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

                    _notifiableItemList.Add(item);
                    item.Notified = DateTime.Now;
                    break;
                }
            }
        }

        public async Task NotifyUsersOfItemsByEmail()
        {
            if (_notifiableItemList.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();

                var orderItemToNotifyList = _notifiableItemList.OrderBy(item => item.Price);
                _logger.Information($"{orderItemToNotifyList.Count()} Items to Notify");

                foreach (var item in orderItemToNotifyList)
                {
                    var probability = item.UnlockProbabilityId == 3 ? Enums.UnlockProbability.High.ToString() : Enums.UnlockProbability.Middle.ToString();

                    stringBuilder.AppendFormat(
                        @"<div style=""margin: 50px 20px;border-radius: 10px;box-shadow: 0px 0px 20px 0px rgba(0, 0, 0, 0.4);padding: 10px;"">
                        <h2 style='margin:0;'>{0}</h2>
                        <p style ='font-size:large; margin:0;'>US$ {1}</p>
                        <p style ='font-size:large; margin:0;'>Unlock Probability: {5}</p>
                        <a href='https://10.0.0.3:8081/api/Items/{4}/cancel-notification' style= 'display:block'> Cancel Notification</a>
                        <a href='{2}'>
                            <img src='{3}' style=""width: 540px;height: 720px;object-fit: cover;""/>
                        </a>
                        </div>", item.Name, item.Price, item.Link, item.Image, item.PublicId, probability);
                }
                string body = stringBuilder.ToString();

                var email = new EmailDto
                {
                    To = "pupyfrias@gmail.com",
                    Subject = "Item Deals",
                    Body = body
                };

                await _emailService.SendEmailAsync(email);
                _notifiableItemList.Clear();
            }
            else
            {
                _logger.Information("There is not items to Notify");
            }
        }
    }
}

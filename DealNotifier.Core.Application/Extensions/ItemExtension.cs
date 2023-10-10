using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Heplers;
using DealNotifier.Core.Application.Utilities;
using DealNotifier.Core.Application.ViewModels.V1.Item;

namespace DealNotifier.Core.Application.Extensions
{
    public static class ItemExtension
    {
        /// <summary>
        /// Validate if the Item can be save.
        /// </summary>
        /// <returns>true if the Item is not in BlackList; otherwise, false.</returns>
        public static async Task<bool> CanBeSaved(this ItemCreateRequest item)
        {
            bool isBanned = false;
            bool isInBlackList = false;

            Task bannedTask = Task.Run(() =>
            {
                isBanned = CacheDataUtility.BanKeywordList.Any(element => item.Name.IndexOf(element.Keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            });

            Task blackListTask = Task.Run(() =>
            {
                isInBlackList = CacheDataUtility.BanLinkList.Any(x => x.Link == item.Link);
            });

            await Task.WhenAll(bannedTask, blackListTask);

            return !(isBanned || isInBlackList);
        }

        public static void SetPhoneCarrier(this ItemCreateRequest item)
        {
            foreach (var phoneCarrier in CacheDataUtility.PhoneCarrierList)
            {
                var slitPhoneCarrierName = phoneCarrier.Name.Split('|');

                foreach (string name in slitPhoneCarrierName)
                {
                    bool isMatched = item.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1;
                    if (isMatched)
                    {
                        item.PhoneCarrierId = phoneCarrier.Id;
                        return;
                    }
                }
            }

            if (item.PhoneCarrierId == null)
            {
                item.PhoneCarrierId = (int)PhoneCarrier.UNK;
            }
        }

        /// <summary>
        /// Set Item Condition
        /// </summary>
        public static void SetCondition(this ItemCreateRequest item)
        {
            /* bool isNotNew =  conditionList.Any(condition =>  item.Name.IndexOf(condition, StringComparison.OrdinalIgnoreCase) >= 0);
             item.ConditionId = isNotNew ? (int)Condition.Used : (int)Condition.New;*/
        }
    }
}
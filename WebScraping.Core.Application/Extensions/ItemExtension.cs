using WebScraping.Core.Application.Heplers;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Extensions
{
    public static class ItemExtension
    {
        /// <summary>
        /// Validate if the Item can be save.
        /// </summary>
        /// <returns>true if the Item is not in BlackList; otherwise, false.</returns>
        public static async Task<bool> CanBeSaved(this Item item)
        {
            bool isBanned = false;
            bool isInBlackList = false;

            Task bannedTask = Task.Run(() =>
            {
                isBanned = Helper.BannedKeywordList.Any(element => item.Name.IndexOf(element.Keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            });

            Task blackListTask = Task.Run(() =>
            {
                isInBlackList = Helper.BlacklistedLinks.Any(x => x.Link == item.Link);
            });

            await Task.WhenAll(bannedTask, blackListTask);

            return !(isBanned || isInBlackList);
        }

        /// <summary>
        /// Set Item Condition
        /// </summary>
        public static void SetCondition(this Item item)
        {
            /* bool isNotNew =  conditionList.Any(condition =>  item.Name.IndexOf(condition, StringComparison.OrdinalIgnoreCase) >= 0);
             item.ConditionId = isNotNew ? (int)Condition.Used : (int)Condition.New;*/
        }
    }
}
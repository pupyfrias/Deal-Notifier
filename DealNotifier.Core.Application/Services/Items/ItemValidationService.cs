using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services.Items
{
    public class ItemValidationService : IItemValidationService
    {
        private readonly ICacheDataService _cacheDataService;
        public ItemValidationService(ICacheDataService cacheDataService)
        {
            _cacheDataService = cacheDataService;
        }


        #region Public Method

        public bool CanBeSaved(ItemDto itemCreate)
        {
            var hasBanKeywords = HasBanKeywords(itemCreate);
            var isLinkBanned = IsLinkBanned(itemCreate);
            var isChecked = _cacheDataService.CheckedList.Any(x => x == itemCreate.Link);

            return !(hasBanKeywords || isLinkBanned || isChecked );
        }

        public bool ContainsWordUnlocked(string description)
        {
            return description.Contains("unlocked", StringComparison.OrdinalIgnoreCase) || description.Contains("ulk",
                StringComparison.OrdinalIgnoreCase);
        }

        public bool DescriptionMatchesIncludeExcludeCriteria(string description, string includeKeywords, string excludeKeywords)
        {
            var includeKeywordList = includeKeywords.Split(',')
                                                    .Select(keyword => keyword.Trim())
                                                    .Where(keyword => !string.IsNullOrEmpty(keyword));
            var excludeKeywordList = excludeKeywords.Split(',')
                                                    .Select(keyword => keyword.Trim())
                                                    .Where(keyword => !string.IsNullOrEmpty(keyword));

            bool includes = includeKeywordList.Any(keyword => description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            bool excludes = excludeKeywordList.Any(keyword => description.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            return (includes && !excludes) || (string.IsNullOrEmpty(includeKeywords) && !excludes);
        }

        public bool CanItemBeUpdated(Item oldItem, ItemDto item)
        {
            return oldItem.Price != item.Price 
                   || oldItem.UnlockProbabilityId != item.UnlockProbabilityId 
                   || oldItem.Image != item.Image 
                   || item.IsAuction 
                   || oldItem.Title != item.Title
                   || oldItem.ShortDescription != item.ShortDescription;
        }

        #endregion Public Method

        #region Private Method

        private bool HasBanKeywords(ItemDto itemCreate)
        {
            return _cacheDataService.BanKeywordList.Any(element => itemCreate.Title.Contains(element.Keyword,
                StringComparison.OrdinalIgnoreCase));
        }

        private bool IsLinkBanned(ItemDto itemCreate)
        {
            return _cacheDataService.BanLinkList.Any(x => x.Link == itemCreate.Link);
        }

        #endregion Private Method
    }
}
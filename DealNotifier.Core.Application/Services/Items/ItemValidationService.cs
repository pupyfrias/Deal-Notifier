using Catalog.Application.Interfaces.Services;
using Catalog.Application.Interfaces.Services.Items;
using Catalog.Application.ViewModels.V1.Item;
using Catalog.Domain.Entities;

namespace Catalog.Application.Services.Items
{
    public class ItemValidationService : IItemValidationService
    {
        private readonly ICacheDataService _cacheDataService;
        public ItemValidationService(ICacheDataService cacheDataService)
        {
            _cacheDataService = cacheDataService;
        }


        #region Public Method

        public bool CanBeSaved(ItemCreateRequest itemCreate)
        {
            var hasBanKeywords = HasBanKeywords(itemCreate);
            var isLinkBanned = IsLinkBanned(itemCreate);
            return !(hasBanKeywords || isLinkBanned);
        }

        public bool ContainsWordUnlocked(string title)
        {
            return title.Contains("unlocked", StringComparison.OrdinalIgnoreCase) || title.Contains("ulk",
                StringComparison.OrdinalIgnoreCase);
        }

        public bool TitleContainsKeyword(string title, string keywords)
        {
            var keywordList = keywords.Split(',').Select(keyword => keyword.Trim());
            return keywordList.Any(keyword => title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        public bool CanItemBeUpdated(Item oldItem, ItemCreateRequest item)
        {
            return oldItem.Price != item.Price || oldItem.UnlockProbabilityId != item.UnlockProbabilityId ||
                   oldItem.Image != item.Image || item.IsAuction;
        }

        #endregion Public Method

        #region Private Method

        private bool HasBanKeywords(ItemCreateRequest itemCreate)
        {
            return _cacheDataService.BanKeywordList.Any(element => itemCreate.Name.Contains(element.Keyword,
                StringComparison.OrdinalIgnoreCase));
        }

        private bool IsLinkBanned(ItemCreateRequest itemCreate)
        {
            return _cacheDataService.BanLinkList.Any(x => x.Link == itemCreate.Link);
        }

        #endregion Private Method
    }
}
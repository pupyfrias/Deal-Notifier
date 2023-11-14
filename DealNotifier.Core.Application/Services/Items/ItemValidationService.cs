﻿using DealNotifier.Core.Application.Interfaces.Services;
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

        public bool CanItemBeUpdated(Item oldItem, ItemDto item)
        {
            return oldItem.Price != item.Price || oldItem.UnlockProbabilityId != item.UnlockProbabilityId ||
                   oldItem.Image != item.Image || item.IsAuction;
        }

        #endregion Public Method

        #region Private Method

        private bool HasBanKeywords(ItemDto itemCreate)
        {
            return _cacheDataService.BanKeywordList.Any(element => itemCreate.Name.Contains(element.Keyword,
                StringComparison.OrdinalIgnoreCase));
        }

        private bool IsLinkBanned(ItemDto itemCreate)
        {
            return _cacheDataService.BanLinkList.Any(x => x.Link == itemCreate.Link);
        }

        #endregion Private Method
    }
}
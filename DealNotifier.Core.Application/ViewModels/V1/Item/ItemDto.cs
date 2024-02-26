﻿using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.Item
{
    public class ItemDto
    {
        public int BidCount { get; set; }
        [Required]
        public int ConditionId { get; set; }
        [Required]
        public string Image { get; set; }
        public bool IsAuction { get; set; }
        public DateTime? ItemEndDate { get; set; }
        [Required]
        public int? ItemTypeId { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public string Title { get; set; }
        public bool Notify { get; set; } = true;
        [Required]
        public int? OnlineStoreId { get; set; }

        public decimal Price { get; set; }
        [Required]
        public int? StockStatusId { get; set; }
        public int UnlockProbabilityId { get; set; }
        public int? UnlockabledPhoneId { get; set; }

        public int BrandId { get; set; }

        public string ShortDescription { get; set; }

    }
}
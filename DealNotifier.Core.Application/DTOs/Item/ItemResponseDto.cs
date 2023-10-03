﻿

namespace DealNotifier.Core.Application.DTOs.Item
{
    public class ItemResponseDto
    {
        public string Id { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Saving { get; set; }
        public decimal SavingsPercentage { get; set; }
        public int ConditionId { get; set; }
        public int ShopId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public DateTime? ItemEndDate { get; set; }
        public int BidCount { get; set; }
        public bool IsAuction { get; set; }
        public int UnlockProbabilityId { get; set; }
        public string ConditionName { get; set; }
        public string ShopName { get; set; }
        public string StatusName { get; set; }
        public string TypeName { get; set; }
        public string UnlockProbabilityName { get; set; }

    }
}
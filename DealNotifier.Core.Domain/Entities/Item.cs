using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Item : AuditableEntity
    {
        public int BidCount { get; set; }
        public int ConditionId { get; set; }
        public Guid PublicId { get; set; }
        public string Image { get; set; }
        public bool? IsAuction { get; set; }
        public DateTime? ItemEndDate { get; set; }
        public int ItemTypeId { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public DateTime? Notified { get; set; }
        public bool? Notify { get; set; }
        public decimal OldPrice { get; set; }
        public int OnlineStoreId { get; set; }
        public decimal Price { get; set; }
        public decimal Saving { get; set; }
        public decimal SavingsPercentage { get; set; }
        public int StockStatusId { get; set; }
        public int? UnlockProbabilityId { get; set; }
        public int? UnlockabledPhoneId { get; set; }
        public UnlockabledPhone UnlockabledPhone { get; set; }
        public Condition Condition { get; set; }
        public ItemType ItemType { get; set; }
        public OnlineStore OnlineStore { get; set; }
        public StockStatus StockStatus { get; set; }
        public UnlockProbability UnlockProbability { get; set; }
    }
}
using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class Item : AuditableEntity<Guid>
    {
        public override Guid Id { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Saving { get; set; }
        public decimal SavingsPercentage { get; set; }
        public int ConditionId { get; set; }
        public int OnlineStoreId { get; set; }
        public int StockStatusId { get; set; }
        public int ItemTypeId { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public int? PhoneCarrierId { get; set; }
        public string? ModelNumber { get; set; }
        public string? ModelName { get; set; }
        public bool Notify { get; set; }
        public bool IsAuction { get; set; }
        public int BidCount { get; set; }
        public int? UnlockProbabilityId { get; set; }
        public DateTime? ItemEndDate { get; set; }
        public DateTime? Notified { get; set; }
        public Condition Condition { get; set; }
        public OnlineStore OnlineStore { get; set; }
        public StockStatus StockStatus { get; set; }
        public ItemType ItemType { get; set; }
        public Brand Brand { get; set; }
        public PhoneCarrier PhoneCarrier { get; set; }
        public UnlockProbability UnlockProbability { get; set; }
    }
}
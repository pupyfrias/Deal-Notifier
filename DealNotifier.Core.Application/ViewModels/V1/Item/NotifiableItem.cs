namespace DealNotifier.Core.Application.ViewModels.V1.Item
{
    public class NotifiableItem
    {
        public int BidCount { get; set; }
        public int ConditionId { get; set; }
        public Guid PublicId { get; set; }
        public string Image { get; set; }
        public bool? IsAuction { get; set; }
        public DateTime? ItemEndDate { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public bool? Notify { get; set; }
        public decimal OldPrice { get; set; }
        public int OnlineStoreId { get; set; }
        public decimal Price { get; set; }
        public decimal Saving { get; set; }
        public decimal SavingsPercentage { get; set; }
        public int UnlockProbabilityId { get; set; }
        public int? UnlockabledPhoneId { get; set; }
        public int BrandId { get; set; }
        public string ShortDescription { get; set; }
        public string[]? UnlockTools { get; set; }
    }
}
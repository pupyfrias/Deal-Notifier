namespace DealNotifier.Core.Application.ViewModels.V1.Item
{
    public class ItemResponse
    {
        public string Id { get; set; }
        public int BidCount { get; set; }
        public int BrandId { get; set; }
        public int ConditionId { get; set; }
        public string ConditionName { get; set; }
        public string Image { get; set; }
        public bool IsAuction { get; set; }
        public DateTime? ItemEndDate { get; set; }
        public string Link { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public string Name { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Saving { get; set; }
        public decimal SavingsPercentage { get; set; }
        public int PhoneCarrierId { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int UnlockProbabilityId { get; set; }
        public string UnlockProbabilityName { get; set; }
        public string PhoneCarrierName { get; set; }
    }
}
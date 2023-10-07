namespace DealNotifier.Core.Application.ViewModels.V1.Item
{
    public class ItemCreateRequest
    {
        public int BidCount { get; set; }
        public int BrandId { get; set; }
        public int ConditionId { get; set; }
        public string Image { get; set; }
        public bool IsAuction { get; set; }
        public DateTime? ItemEndDate { get; set; }
        public string Link { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public string Name { get; set; }
        public bool Notify { get; set; } = true;
        public decimal OldPrice { get; set; }
        public int PhoneCarrierId { get; set; }
        public decimal Price { get; set; }
        public int ShopId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public int UnlockProbabilityId { get; set; }
    }
}
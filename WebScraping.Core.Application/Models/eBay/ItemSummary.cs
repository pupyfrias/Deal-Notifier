namespace WebScraping.Core.Application.Models.eBay
{
    public class ItemSummary
    {
        public List<AdditionalImage> AdditionalImages { get; set; }
        public bool AdultOnly { get; set; }
        public bool AvailableCoupons { get; set; }
        public int BidCount { get; set; }
        public List<string> BuyingOptions { get; set; }
        public List<Category> Categories { get; set; }
        public string CompatibilityMatch { get; set; }
        public List<CompatibilityProperty> CompatibilityProperties { get; set; }
        public string Condition { get; set; }
        public string ConditionId { get; set; }
        public CurrentBidPrice CurrentBidPrice { get; set; }
        public DistanceFromPickupLocation DistanceFromPickupLocation { get; set; }
        public string EnergyEfficiencyClass { get; set; }
        public string Epid { get; set; }
        public Image Image { get; set; }
        public string ItemAffiliateWebUrl { get; set; }
        public string ItemCreationDate { get; set; }
        public string ItemEndDate { get; set; }
        public string ItemGroupHref { get; set; }
        public string ItemGroupType { get; set; }
        public string ItemHref { get; set; }
        public string ItemId { get; set; }
        public string ItemWebUrl { get; set; }
        public ItemLocation ItemLocation { get; set; }
        public List<string> leafCategoryIds { get; set; }
        public string LegacyItemId { get; set; }
        public string ListingMarketplaceId { get; set; }
        public MarketingPrice MarketingPrice { get; set; }
        public List<PickupOption> PickupOptions { get; set; }
        public Price Price { get; set; }
        public string PriceDisplayCondition { get; set; }
        public bool PriorityListingy { get; set; }
        public List<string> QualifiedPrograms { get; set; }
        public Seller Seller { get; set; }
        public List<ShippingOption> ShippingOptions { get; set; }
        public string ShortDescription { get; set; }
        public List<ThumbnailImage> ThumbnailImages { get; set; }
        public string Title { get; set; }
        public bool TopRatedBuyingExperience { get; set; }
        public string TyreLabelImageUrl { get; set; }
        public UnitPrice UnitPrice { get; set; }
        public string UnitPricingMeasure { get; set; }
        public int WatchCount { get; set; }
    }
}
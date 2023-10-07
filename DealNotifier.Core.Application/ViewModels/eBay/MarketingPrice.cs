namespace DealNotifier.Core.Application.ViewModels.eBay
{
    public class MarketingPrice
    {
        public DiscountAmount DiscountAmount { get; set; }
        public string DiscountPercentage { get; set; }
        public OriginalPrice OriginalPrice { get; set; }
        public string PriceTreatment { get; set; }
    }
}
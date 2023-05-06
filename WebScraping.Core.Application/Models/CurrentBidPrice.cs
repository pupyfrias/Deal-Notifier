namespace WebScraping.Core.Application.Models
{
    public class CurrentBidPrice
    {
        public string ConvertedFromCurrency { get; set; }
        public string ConvertedFromValue { get; set; }
        public string Currency { get; set; }
        public string Value { get; set; }
    }
}
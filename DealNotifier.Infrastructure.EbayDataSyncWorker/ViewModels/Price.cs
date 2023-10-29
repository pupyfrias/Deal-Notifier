namespace DealNotifier.Infrastructure.EbayDataSyncWorker.ViewModels
{
    public class Price
    {
        public string ConvertedFromCurrency { get; set; }
        public string ConvertedFromValue { get; set; }
        public string Currency { get; set; }
        public string Value { get; set; }
    }
}
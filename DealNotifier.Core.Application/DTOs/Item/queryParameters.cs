namespace DealNotifier.Core.Application.DTOs.Item
{
    public class QueryParameters
    {
        public string? Brands { get; set; }
        public string? Storages { get; set; }
        public string? Search { get; set; }
        public string? Max { get; set; }
        public string? Min { get; set; }
        public string? Sort_by { get; set; }
        public string? Offer { get; set; }
        public string? Types { get; set; }
        public string? Condition { get; set; }
        public string? Carriers { get; set; }
        public string? Excludes { get; set; }
        public string? Shops { get; set; }
    }
}
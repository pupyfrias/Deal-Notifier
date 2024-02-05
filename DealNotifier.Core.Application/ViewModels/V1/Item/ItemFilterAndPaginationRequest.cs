using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.Item
{
    public class ItemFilterAndPaginationRequest : PaginationBase
    {
        public string? Brands { get; set; }
        public string? Conditions { get; set; }
        public string? Excludes { get; set; }
        public string? Max { get; set; }
        public string? Min { get; set; }
        public string? Offer { get; set; }
        public string? PhoneCarriers { get; set; }
        public string? Search { get; set; }
        public string? Stores { get; set; }
        public string? Sort_by { get; set; }
        public string? Storages { get; set; }
        public string? Types { get; set; }
        public string? UnlockProbabilities { get; set; }
    }
}
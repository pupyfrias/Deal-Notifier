using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.NotificationCriteria
{
    public class NotificationCriteriaFilterAndPaginationRequest : PaginationBase
    {
        public string? Keywords { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Condition { get; set; }
    }
}
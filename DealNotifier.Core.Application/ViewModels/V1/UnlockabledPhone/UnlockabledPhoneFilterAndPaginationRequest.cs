using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone
{
    public class UnlockabledPhoneFilterAndPaginationRequest : PaginationBase
    {
        public int? BrandId { get; set; }
        public string? ModelName { get; set; }
        public string? ModelNumber { get; set; }
    }
}
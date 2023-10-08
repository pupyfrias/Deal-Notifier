using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool
{
    public class PhoneUnlockToolFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
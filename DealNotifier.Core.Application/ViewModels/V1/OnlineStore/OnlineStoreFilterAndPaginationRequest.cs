using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.OnlineStore
{
    public class OnlineStoreFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.StockStatus
{
    public class StockStatusFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
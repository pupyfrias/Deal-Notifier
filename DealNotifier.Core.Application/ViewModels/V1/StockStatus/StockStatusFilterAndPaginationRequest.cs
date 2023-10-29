using Catalog.Application.Wrappers;

namespace Catalog.Application.ViewModels.V1.StockStatus
{
    public class StockStatusFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
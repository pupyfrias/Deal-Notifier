using Catalog.Application.Wrappers;

namespace Catalog.Application.ViewModels.V1.ItemType
{
    public class ItemTypeFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.ItemType
{
    public class ItemTypeFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
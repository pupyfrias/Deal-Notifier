using Catalog.Application.Wrappers;

namespace Catalog.Application.ViewModels.V1.OnlineStore
{
    public class OnlineStoreFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
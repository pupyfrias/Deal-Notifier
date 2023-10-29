using Catalog.Application.Wrappers;

namespace Catalog.Application.ViewModels.V1.Brand
{
    public class BrandFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
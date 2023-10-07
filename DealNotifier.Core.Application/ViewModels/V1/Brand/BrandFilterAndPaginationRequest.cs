using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.Brand
{
    public class BrandFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
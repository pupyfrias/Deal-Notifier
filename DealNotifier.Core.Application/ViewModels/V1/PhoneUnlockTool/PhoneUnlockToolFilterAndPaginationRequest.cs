using Catalog.Application.Wrappers;

namespace Catalog.Application.ViewModels.V1.PhoneUnlockTool
{
    public class PhoneUnlockToolFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
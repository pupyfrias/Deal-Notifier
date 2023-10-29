using Catalog.Application.Wrappers;

namespace Catalog.Application.ViewModels.V1.UnlockabledPhone
{
    public class UnlockabledPhoneFilterAndPaginationRequest : PaginationBase
    {
        public int? BrandId { get; set; }
        public string? ModelName { get; set; }
        public string? ModelNumber { get; set; }
    }
}
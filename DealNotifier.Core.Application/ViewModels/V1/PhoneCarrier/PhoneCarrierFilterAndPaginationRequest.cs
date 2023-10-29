using Catalog.Application.Wrappers;

namespace Catalog.Application.ViewModels.V1.PhoneCarrier
{
    public class PhoneCarrierFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
    }
}
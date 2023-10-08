using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier
{
    public class PhoneCarrierFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
    }
}
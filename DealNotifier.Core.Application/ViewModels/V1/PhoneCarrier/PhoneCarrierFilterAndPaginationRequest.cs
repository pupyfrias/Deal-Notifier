using DealNotifier.Core.Application.ViewModels.Common;

namespace WebApi.Controllers.V1
{
    public class PhoneCarrierFilterAndPaginationRequest: PaginationBase
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
    }
}
using DealNotifier.Core.Application.ViewModels.Common;

namespace WebApi.Controllers.V1
{
    public class OnlineStoreFilterAndPaginationRequest: PaginationBase
    {
        public string? Name { get; set; }
    }
}
using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.UnlockProbability
{
    public class UnlockProbabilityFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
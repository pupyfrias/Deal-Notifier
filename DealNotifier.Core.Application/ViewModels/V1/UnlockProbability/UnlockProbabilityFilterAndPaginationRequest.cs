using Catalog.Application.Wrappers;

namespace Catalog.Application.ViewModels.V1.UnlockProbability
{
    public class UnlockProbabilityFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
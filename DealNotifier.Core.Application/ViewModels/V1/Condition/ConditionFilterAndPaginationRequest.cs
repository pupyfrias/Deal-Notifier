using Catalog.Application.Wrappers;

namespace Catalog.Application.ViewModels.V1.Condition
{
    public class ConditionFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.Condition
{
    public class ConditionFilterAndPaginationRequest : PaginationBase
    {
        public string? Name { get; set; }
    }
}
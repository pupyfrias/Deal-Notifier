using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.ViewModels.V1.BanKeyword
{
    public class BanKeywordFilterAndPaginationRequest : PaginationBase
    {
        public string? Keyword { get; set; }
    }
}
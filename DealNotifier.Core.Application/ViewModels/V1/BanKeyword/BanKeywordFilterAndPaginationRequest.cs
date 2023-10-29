using Catalog.Application.Wrappers;

namespace Catalog.Application.ViewModels.V1.BanKeyword
{
    public class BanKeywordFilterAndPaginationRequest : PaginationBase
    {
        public string? Keyword { get; set; }
    }
}
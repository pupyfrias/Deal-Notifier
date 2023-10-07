using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;

namespace WebApi.Controllers.V1
{
    public class StockStatusSpecification: Specification<StockStatus>
    {
        public StockStatusSpecification(StockStatusFilterAndPaginationRequest request):base (request)
        {
            #region Name
            if (request.Name != null)
            {
                Expression<Func<StockStatus, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria =  expression;
            }
            #endregion Name
        }
    }
}
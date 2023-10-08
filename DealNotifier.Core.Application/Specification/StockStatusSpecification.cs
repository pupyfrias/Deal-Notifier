using DealNotifier.Core.Application.ViewModels.V1.StockStatus;
using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Specification
{
    public class StockStatusSpecification : Specification<StockStatus>
    {
        public StockStatusSpecification(StockStatusFilterAndPaginationRequest request) : base(request)
        {
            #region Name
            if (request.Name != null)
            {
                Expression<Func<StockStatus, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria = expression;
            }
            #endregion Name
        }
    }
}
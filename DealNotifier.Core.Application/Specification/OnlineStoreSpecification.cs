using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;

namespace WebApi.Controllers.V1
{
    public class OnlineStoreSpecification: Specification<OnlineStore>
    {
        public OnlineStoreSpecification(OnlineStoreFilterAndPaginationRequest request) :base(request)
        {
            #region Name
            if (request.Name != null)
            {
                Expression<Func<OnlineStore, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Name
        }
    }
}
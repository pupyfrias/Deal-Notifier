using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.NotificationCriteria;
using Catalog.Domain.Entities;
using System.Linq.Expressions;

namespace Catalog.Application.Specification
{
    public class NotificationCriteriaSpecification : Specification<NotificationCriteria>
    {
        public NotificationCriteriaSpecification(NotificationCriteriaFilterAndPaginationRequest request) : base(request)
        {

            #region Condition
            if (request.Condition != null)
            {
                Expression<Func<NotificationCriteria, bool>> expression = item => item.ConditionId.Equals(request.Condition);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Condition


            #region Keywords
            if (request.Keywords != null)
            {
                Expression<Func<NotificationCriteria, bool>> expression = item => item.Keywords.Contains(request.Keywords);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Keywords

            #region MaxPrice

            if (request.MaxPrice != null)
            {
                Expression<Func<NotificationCriteria, bool>> expression = item => item.MaxPrice < request.MaxPrice;
                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }

            #endregion MaxPrice
        }
    }
}
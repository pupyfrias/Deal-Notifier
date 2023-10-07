using DealNotifier.Core.Application.ViewModels.V1.Condition;
using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Specification
{
    public class ConditionSpecification : Specification<Condition>
    {
        public ConditionSpecification(ConditionFilterAndPaginationRequest request) : base(request)
        {
            #region Name

            if (request.Name != null)
            {
                Expression<Func<Condition, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria = expression;
            }

            #endregion Name
        }
    }
}
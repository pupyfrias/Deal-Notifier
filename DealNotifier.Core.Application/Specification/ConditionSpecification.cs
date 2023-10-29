using Catalog.Application.ViewModels.V1.Condition;
using Catalog.Domain.Entities;
using System.Linq.Expressions;

namespace Catalog.Application.Specification
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
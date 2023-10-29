using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.PhoneUnlockTool;
using Catalog.Domain.Entities;
using System.Linq.Expressions;

namespace Catalog.Application.Specification
{
    public class PhoneUnlockToolSpecification : Specification<PhoneUnlockTool>
    {
        public PhoneUnlockToolSpecification(PhoneUnlockToolFilterAndPaginationRequest request) : base(request)
        {
            #region Name
            if (request.Name != null)
            {
                Expression<Func<PhoneUnlockTool, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Name
        }
    }
}
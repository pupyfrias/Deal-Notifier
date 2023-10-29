using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.UnlockabledPhone;
using Catalog.Domain.Entities;
using System.Linq.Expressions;

namespace Catalog.Application.Specification
{
    public class UnlockabledPhoneSpecification : Specification<UnlockabledPhone>
    {
        public UnlockabledPhoneSpecification(UnlockabledPhoneFilterAndPaginationRequest request) : base(request)
        {
            #region ModelName
            if (request.ModelName != null)
            {
                Expression<Func<UnlockabledPhone, bool>> expression = item => item.ModelName.Contains(request.ModelName);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion ModelName


            #region ModelNumber
            if (request.ModelNumber != null)
            {
                Expression<Func<UnlockabledPhone, bool>> expression = item => item.ModelNumber.Contains(request.ModelNumber);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion ModelNumber

            #region BrandId
            if (request.BrandId != null)
            {
                Expression<Func<UnlockabledPhone, bool>> expression = item => item.BrandId.Equals(request.BrandId);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion BrandId
        }
    }

}
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.PhoneCarrier;
using Catalog.Domain.Entities;
using System.Linq.Expressions;

namespace Catalog.Application.Specification
{
    public class PhoneCarrierSpecification : Specification<PhoneCarrier>
    {
        public PhoneCarrierSpecification(PhoneCarrierFilterAndPaginationRequest request) : base(request)
        {
            #region Name
            if (request.Name != null)
            {
                Expression<Func<PhoneCarrier, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion Name

            #region ShortName
            if (request.ShortName != null)
            {
                Expression<Func<PhoneCarrier, bool>> expression = item => item.ShortName.Contains(request.ShortName);

                Criteria = Criteria is null ? expression : Criteria.And(expression);
            }
            #endregion ShortName
        }
    }
}
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.OnlineStore;
using Catalog.Domain.Entities;
using System.Linq.Expressions;

namespace Catalog.Application.Specification
{
    public class OnlineStoreSpecification : Specification<OnlineStore>
    {
        public OnlineStoreSpecification(OnlineStoreFilterAndPaginationRequest request) : base(request)
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
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.Brand;
using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Specification
{
    public class BrandSpecification : Specification<Brand>
    {
        public BrandSpecification(BrandFilterAndPaginationRequest request) : base(request)
        {

            #region Name

            if (request.Name != null)
            {
                Expression<Func<Brand, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria = expression ;
            }

            #endregion Name
        }
    }
}
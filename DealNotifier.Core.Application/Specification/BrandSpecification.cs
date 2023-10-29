using Catalog.Application.ViewModels.V1.Brand;
using Catalog.Domain.Entities;
using System.Linq.Expressions;

namespace Catalog.Application.Specification
{
    public class BrandSpecification : Specification<Brand>
    {
        public BrandSpecification(BrandFilterAndPaginationRequest request) : base(request)
        {

            #region Name

            if (request.Name != null)
            {
                Expression<Func<Brand, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria = expression;
            }

            #endregion Name
        }
    }
}
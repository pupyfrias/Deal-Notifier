using Catalog.Application.ViewModels.V1.BanLink;
using Catalog.Domain.Entities;

namespace Catalog.Application.Specification
{
    public class BanLinkSpecification : Specification<BanLink>
    {
        public BanLinkSpecification(BanLinkPaginationRequest request) : base(request)
        {

        }
    }
}
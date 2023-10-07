using DealNotifier.Core.Application.Specification;
using DealNotifier.Core.Application.ViewModels.V1.BanLink;
using DealNotifier.Core.Domain.Entities;

namespace WebApi.Controllers.V1
{
    public class BanLinkSpecification: Specification<BanLink>
    {
        public BanLinkSpecification(BanLinkPaginationRequest request): base(request)
        {
            
        }
    }
}
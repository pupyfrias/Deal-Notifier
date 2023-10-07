using DealNotifier.Core.Application.ViewModels.V1.BanLink;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Specification
{
    public class BanLinkSpecification : Specification<BanLink>
    {
        public BanLinkSpecification(BanLinkPaginationRequest request) : base(request)
        {
            
        }
    }
}
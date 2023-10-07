using DealNotifier.Core.Application.ViewModels.V1.ItemType;
using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Specification
{
    public class ItemTypeSpecification : Specification<ItemType>
    {
        public ItemTypeSpecification(ItemTypeFilterAndPaginationRequest request) : base(request)
        {
            #region Name

            if (request.Name != null)
            {
                Expression<Func<ItemType, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria = expression;
            }

            #endregion Name
        }
    }
}
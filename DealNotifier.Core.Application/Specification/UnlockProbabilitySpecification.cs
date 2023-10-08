using DealNotifier.Core.Application.ViewModels.V1.UnlockProbability;
using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Specification
{
    public class UnlockProbabilitySpecification : Specification<UnlockProbability>
    {
        public UnlockProbabilitySpecification(UnlockProbabilityFilterAndPaginationRequest request) : base(request)
        {
            #region Name
            if (request.Name != null)
            {
                Expression<Func<UnlockProbability, bool>> expression = item => item.Name.Contains(request.Name);

                Criteria = expression;
            }
            #endregion Name
        }
    }
}
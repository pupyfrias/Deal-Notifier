using DealNotifier.Core.Application.Interfaces;

namespace DealNotifier.Core.Application.ViewModels.V1.NotificationCriteria
{
    public class NotificationCriteriaUpdateRequest : IHasId<int>
    {
        public int Id { get; set; }
        public string Keywords { get; set; }
        public decimal MaxPrice { get; set; }
        public int ConditionId { get; set; }
    }
}
namespace DealNotifier.Core.Application.ViewModels.V1.NotificationCriteria
{
    public class NotificationCriteriaCreateRequest
    {
        public string Keywords { get; set; }
        public decimal MaxPrice { get; set; }
        public int ConditionId { get; set; }
    }
}
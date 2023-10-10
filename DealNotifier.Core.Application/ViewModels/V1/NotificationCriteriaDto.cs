namespace DealNotifier.Core.Application.ViewModels.V1
{
    public class NotificationCriteriaDto
    {
        public string Keywords { get; set; }
        public decimal MaxPrice { get; set; }
        public int ConditionId { get; set; }
    }
}
namespace DealNotifier.Core.Application.ViewModels.V1
{
    public class NotificationCriteriaDto
    {
        public string IncludeKeywords { get; set; }
        public string ExcludeKeywords { get; set; }
        public decimal MaxPrice { get; set; }
        public int ConditionId { get; set; }
    }
}
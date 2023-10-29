namespace Catalog.Application.ViewModels.V1.NotificationCriteria
{
    public class NotificationCriteriaResponse
    {
        public int Id { get; set; }
        public string Keywords { get; set; }
        public decimal MaxPrice { get; set; }
        public int ConditionId { get; set; }
        public string ConditionName { get; set; }
    }
}
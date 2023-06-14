namespace DealNotifier.Core.Application.DTOs
{
    public class ConditionsToNotifyDto
    {
        public string Keywords { get; set; }
        public decimal MaxPrice { get; set; }
        public int ConditionId { get; set; }
    }
}
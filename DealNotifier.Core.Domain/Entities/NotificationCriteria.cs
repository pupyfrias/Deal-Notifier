using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class NotificationCriteria : EntityBase
    {
        public string IncludeKeywords { get; set; }
        public string ExcludeKeywords { get; set; }
        public decimal MaxPrice { get; set; }
        public int ConditionId { get; set; }
        public Condition Condition { get; set; }
    }
}
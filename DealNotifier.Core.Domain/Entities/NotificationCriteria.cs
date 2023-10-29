using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class NotificationCriteria : AuditableEntity
    {
        public string Keywords { get; set; }
        public decimal MaxPrice { get; set; }
        public int ConditionId { get; set; }
        public Condition Condition { get; set; }
    }
}
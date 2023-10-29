using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Condition : AuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<NotificationCriteria> ConditionToNotifies { get; set; }
    }
}
using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class Condition : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<NotificationCriteria> ConditionToNotifies { get; set; }
    }
}
using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class Condition : EntityBase
    {
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<NotificationCriteria> ConditionToNotifies { get; set; }
    }
}
using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class UnlockProbability : AuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
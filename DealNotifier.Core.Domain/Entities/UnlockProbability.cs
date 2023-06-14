using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class UnlockProbability : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public List<Item> Items { get; set; }
    }
}

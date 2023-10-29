using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class UnlockProbability : AuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
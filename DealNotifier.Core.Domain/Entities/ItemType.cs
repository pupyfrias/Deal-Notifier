using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class ItemType : AuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
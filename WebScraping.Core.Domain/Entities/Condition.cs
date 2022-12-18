using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class Condition: AuditableBaseEntity
    {
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }

    }
}

using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class Status: AuditableBaseEntity
    {
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }

    }
}

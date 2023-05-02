using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class Banned : AuditableBaseEntity
    {
        public int Id { get; }
        public string Keyword { get; set; }
    }
}

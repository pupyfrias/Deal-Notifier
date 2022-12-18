using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class BlackList: AuditableBaseEntity
    {
        public DateTime Date { get; set; }
        public string Link { get; set; }

    }
}

using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class Banned : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Keyword { get; set; }
    }
}
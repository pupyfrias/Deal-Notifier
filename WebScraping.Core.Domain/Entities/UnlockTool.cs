using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class UnlockTool : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public List<UnlockableUnlockTool> UnlockableUnlockTools { get; set; }
    }
}
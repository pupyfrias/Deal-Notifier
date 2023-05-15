using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class Unlockable : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public int BrandId { get; set; }
        public string? Comment { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public Brand Brand { get; set; }
        public List <UnlockablePhoneCarrier> UnlockablePhoneCarriers { get; set; }
        public List <UnlockableUnlockTool> UnlockableUnlockTools { get; set; }
    }
}
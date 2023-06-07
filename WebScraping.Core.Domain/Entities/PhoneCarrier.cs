using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class PhoneCarrier : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public ICollection<Item> Items { get; set; }
        public List<UnlockablePhonePhoneCarrier> UnlockablePhoneCarriers { get; set; }
    }
}
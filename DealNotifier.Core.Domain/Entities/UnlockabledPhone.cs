using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class UnlockabledPhone : AuditableEntity
    {
        public int BrandId { get; set; }
        public string? Comment { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public Brand Brand { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<UnlockabledPhonePhoneCarrier> UnlockabledPhonePhoneCarrier { get; set; }
        public IEnumerable<UnlockabledPhonePhoneUnlockTool> UnlockabledPhoneUnlockTool { get; set; }
    }
}
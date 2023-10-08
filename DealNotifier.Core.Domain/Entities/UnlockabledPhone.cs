using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class UnlockabledPhone : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public int BrandId { get; set; }
        public string? Comment { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public Brand Brand { get; set; }
        public List<UnlockabledPhonePhoneCarrier> UnlockabledPhonePhoneCarrier { get; set; }
        public List<UnlockabledPhoneUnlockTool> UnlockabledPhoneUnlockTool { get; set; }
    }
}
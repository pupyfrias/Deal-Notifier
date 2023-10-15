using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class PhoneUnlockTool : AuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<UnlockabledPhonePhoneUnlockTool> UnlockableUnlockTools { get; set; }
    }
}
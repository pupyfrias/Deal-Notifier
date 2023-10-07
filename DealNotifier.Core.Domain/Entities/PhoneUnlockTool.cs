using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class PhoneUnlockTool : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public List<UnlockabledPhoneUnlockTool> UnlockableUnlockTools { get; set; }
    }
}
using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class PhoneUnlockTool : EntityBase
    {
        public string Name { get; set; }
        public IEnumerable<UnlockabledPhonePhoneUnlockTool> UnlockableUnlockTools { get; set; }
    }
}
using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class PhoneUnlockTool : AuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<UnlockabledPhonePhoneUnlockTool> UnlockableUnlockTools { get; set; }
    }
}
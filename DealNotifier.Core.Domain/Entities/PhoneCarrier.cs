using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class PhoneCarrier : AuditableEntity
    {

        public string Name { get; set; }
        public string ShortName { get; set; }
        public IEnumerable<UnlockabledPhonePhoneCarrier> UnlockablePhoneCarriers { get; set; }
    }
}
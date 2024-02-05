using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class PhoneCarrier : EntityBase
    {
        
        public string Name { get; set; }
        public string ShortName { get; set; }
        public IEnumerable<UnlockabledPhonePhoneCarrier> UnlockablePhoneCarriers { get; set; }
    }
}
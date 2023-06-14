namespace DealNotifier.Core.Domain.Entities
{
    public class UnlockablePhonePhoneCarrier
    {
        public int UnlockablePhoneId { get; set; }
        public int PhoneCarrierId { get; set; }
        public UnlockablePhone Unlockable { get; set; }
        public PhoneCarrier PhoneCarrier { get; set; }

    }
}
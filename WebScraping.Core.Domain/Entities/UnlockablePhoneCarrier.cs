namespace WebScraping.Core.Domain.Entities
{
    public class UnlockablePhoneCarrier
    {
        public int UnlockableId { get; set; }
        public int PhoneCarrierId { get; set; }
        public Unlockable Unlockable { get; set; }
        public PhoneCarrier PhoneCarrier { get; set; }

    }
}
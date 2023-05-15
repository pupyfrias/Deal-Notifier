namespace WebScraping.Core.Domain.Entities
{
    public class UnlockableUnlockTool
    {
        public int UnlockableId { get; set; }
        public int UnlockToolId { get; set; }
        public Unlockable Unlockable { get; set; }
        public UnlockTool UnlockTool { get; set; }

    }
}
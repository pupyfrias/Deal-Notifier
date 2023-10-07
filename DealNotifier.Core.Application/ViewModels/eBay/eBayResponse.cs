namespace DealNotifier.Core.Application.ViewModels.eBay
{
    public class eBayResponse
    {
        public AutoCorrection AutoCorrections { get; set; }
        public string Href { get; set; }
        public List<ItemSummary> ItemSummaries { get; set; }
        public int Limit { get; set; }
        public string Next { get; set; }
        public int Offset { get; set; }
        public string Prev { get; set; }
        public Refinement Refinement { get; set; }
        public int Total { get; set; }

        public List<Warning> Warnings { get; set; }
    }
}
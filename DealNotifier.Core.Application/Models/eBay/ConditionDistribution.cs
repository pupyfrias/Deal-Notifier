namespace DealNotifier.Core.Application.Models.eBay
{
    public class ConditionDistribution
    {
        public string ConditionId { get; set; }
        public string ConditionName { get; set; }
        public string RefinementHref { get; set; }
        public int MatchCount { get; set; }
    }
}
namespace WebScraping.Core.Application.Models
{
    public class CategoryDistribution
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string RefinementHref { get; set; }
        public int MatchCount { get; set; }
    }

}
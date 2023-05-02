using System.Collections.Generic;

namespace WebScraping.Core.Application.Models
{
    public class Refinement
    {
        public List<AspectDistribution> AspectDistributions { get; set; }
        public List<BuyingOptionDistribution> buyingOptionDistributions { get; set; }
        public List<CategoryDistribution> CategoryDistributions { get; set; }
        public List<ConditionDistribution> ConditionDistributions { get; set; }

        public string DominantCategoryId { get; set; }
    }

}
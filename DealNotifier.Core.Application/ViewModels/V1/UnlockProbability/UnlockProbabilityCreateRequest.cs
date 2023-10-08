using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.UnlockProbability
{
    public class UnlockProbabilityCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.UnlockProbability
{
    public class UnlockProbabilityCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
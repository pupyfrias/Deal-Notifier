using DealNotifier.Core.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.UnlockProbability
{
    public class UnlockProbabilityUpdateRequest : IHasId<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
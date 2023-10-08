using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.Condition
{
    public class ConditionCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
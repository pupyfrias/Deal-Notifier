using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.Condition
{
    public class ConditionCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
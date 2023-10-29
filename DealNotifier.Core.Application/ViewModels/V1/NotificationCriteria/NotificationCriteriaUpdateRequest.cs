using Catalog.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.NotificationCriteria
{
    public class NotificationCriteriaUpdateRequest : IHasId<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Keywords { get; set; }
        [Required]
        public decimal MaxPrice { get; set; }
        [Required]
        public int ConditionId { get; set; }
    }
}
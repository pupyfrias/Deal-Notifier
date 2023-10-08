using DealNotifier.Core.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.NotificationCriteria
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
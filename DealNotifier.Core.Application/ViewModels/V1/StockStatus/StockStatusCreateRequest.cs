using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.StockStatus
{
    public class StockStatusCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers.V1
{
    public class StockStatusCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
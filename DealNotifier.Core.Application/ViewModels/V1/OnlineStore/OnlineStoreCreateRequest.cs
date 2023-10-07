using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers.V1
{
    public class OnlineStoreCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Dtos.Auth
{
    public class ConfirmEmailDto
    {
        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? Code { get; set; }
    }
}
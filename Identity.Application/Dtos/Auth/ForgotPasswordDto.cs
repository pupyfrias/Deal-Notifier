using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Dtos.Auth
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
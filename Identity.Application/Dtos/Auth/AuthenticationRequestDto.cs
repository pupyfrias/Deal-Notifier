using System.ComponentModel.DataAnnotations;

namespace Identity.Application.Dtos.Auth
{
    public class AuthenticationRequestDto
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
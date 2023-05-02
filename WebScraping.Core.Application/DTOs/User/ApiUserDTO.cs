using System.ComponentModel.DataAnnotations;

namespace WebScraping.Core.Application.Dtos.User
{
    public class ApiUserDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
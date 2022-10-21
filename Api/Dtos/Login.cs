using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class UserLogin
    {
        [Required(ErrorMessage = "el nombre de usuario es necesario")]
        public string User_name { get; set; }
        [Required(ErrorMessage = "la contraseña es nesecaria")]
        public string Password { get; set; }
    }
}

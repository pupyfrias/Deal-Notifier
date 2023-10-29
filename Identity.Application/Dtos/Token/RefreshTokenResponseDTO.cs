using System.Text.Json.Serialization;

namespace Identity.Application.Dtos.Token
{
    public class RefreshTokenResponseDTO
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
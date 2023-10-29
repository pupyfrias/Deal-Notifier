using System.Text.Json.Serialization;

namespace Catalog.Application.ViewModels.V1.Auth
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public string AccessToken { get; set; }
        public DateTime ValidTo { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
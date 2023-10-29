namespace Identity.Application.Dtos.Token
{
    public class RefreshTokenRequestDTO
    {
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
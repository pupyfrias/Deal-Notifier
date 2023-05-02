namespace WebScraping.Core.Application.Dtos.Token
{
    public class RefreshTokenRequestDto
    {
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
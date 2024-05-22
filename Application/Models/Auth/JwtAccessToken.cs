namespace Api.Application.Models.Auth
{
    public class JwtAccessToken
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}

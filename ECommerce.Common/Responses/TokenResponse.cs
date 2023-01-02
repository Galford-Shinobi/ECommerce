namespace ECommerce.Common.Responses
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public ObtainUserResponse ObtainUser { get; set; }
        public DateTime ExpirationLocal => Expiration.ToLocalTime();
    }
}

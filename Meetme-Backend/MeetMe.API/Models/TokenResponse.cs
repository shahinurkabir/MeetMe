namespace MeetMe.API.Models
{
    public class TokenResponse
    {
        public string Token { get; set; } = null!;
        public double ExpiredAt { get; set; }
    }
}

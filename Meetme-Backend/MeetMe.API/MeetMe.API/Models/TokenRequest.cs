namespace MeetMe.API.Models
{
    public class TokenRequest
    {
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

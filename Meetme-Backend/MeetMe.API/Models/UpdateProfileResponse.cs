namespace MeetMe.API.Models
{
    public class UpdateProfileResponse
    {
        public bool Result { get; set; }
        public TokenResponse NewToken { get; set; }=null!;
    }
}

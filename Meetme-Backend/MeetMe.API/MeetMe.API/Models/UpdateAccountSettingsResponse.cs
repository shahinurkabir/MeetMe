namespace MeetMe.API.Models
{
    public class UpdateAccountSettingsResponse
    {
        public bool Result { get; set; }
        public TokenResponse NewToken { get; set; }=null!;
    }
}

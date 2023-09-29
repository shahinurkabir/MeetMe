using MeetMe.Application.AccountSettings.Dtos;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.API.Models
{
    public class UserProfileDetailResponse
    {
        public AccountProfileDto Profile { get; set; } = null!;
        public List<EventType> Events { get; set; } = null!;
    }
}

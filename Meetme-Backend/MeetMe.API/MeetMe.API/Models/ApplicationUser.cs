using MeetMe.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMe.API.Models
{
    public class ApplicationUser : IUserInfo
    {
        public ApplicationUser()
        {
            UserId = Guid.Parse("6DD70EC9-80BE-41B6-88C4-D9E596B730A8");
            UserName = "Shahinur Kabir Mondo";
            Email = "kmondol@julyservices.com";
            TimeZoneId = "Bangladesh Standard Time";
        }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string TimeZoneId { get; set; }
    }
}

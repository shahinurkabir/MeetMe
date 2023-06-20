using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Interface
{
    public interface IUserInfo
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string BaseURI { get; set; } 
        public string TimeZone { get; set; }

    }
}

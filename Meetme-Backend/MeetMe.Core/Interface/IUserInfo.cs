using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Interface
{
    public interface IUserInfo
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string TimeZoneId { get; set; }

    }
}

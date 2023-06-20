using MeetMe.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMe.API.Models
{
    public class ApplicationUser : IUserInfo
    {
        public ApplicationUser(IHttpContextAccessor httpContextAccessor)
        {

            httpContextAccessor.HttpContext?.User.Claims.ToList().ForEach(x =>
            {
                if (x.Type == "Id")
                {
                    Id =Guid.Parse( x.Value);
                }
                if (x.Type == "UserId")
                {
                    UserId = x.Value;
                }
                if (x.Type == "Email")
                {
                    Email = x.Value;
                }
                if (x.Type == "TimeZone")
                {
                    TimeZone = x.Value;
                }
            });
        }
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string BaseURI { get; set; } = null!;
        public string TimeZone { get; set; }
    }
}

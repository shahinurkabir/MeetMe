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
            //UserId = Guid.Parse("6DD70EC9-80BE-41B6-88C4-D9E596B730A8");
            //UserName = "Shahinur Kabir Mondol";
            //Email = "kmondol@julyservices.com";
            //TimeZoneId = "Bangladesh Standard Time";

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
                if (x.Type == "TimeZoneId")
                {
                    TimeZoneId = int.Parse(x.Value);
                }
            });
        }
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string BaseURI { get; set; } = null!;
        public int TimeZoneId { get; set; }
    }
}

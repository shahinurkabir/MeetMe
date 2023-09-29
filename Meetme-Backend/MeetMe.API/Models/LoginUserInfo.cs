using MeetMe.Core.Constants;
using MeetMe.Core.Interface;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMe.API.Models
{
    public class LoginUserInfo : ILoginUserInfo
    {
        public LoginUserInfo(IHttpContextAccessor httpContextAccessor)
        {

            httpContextAccessor.HttpContext?.User.Claims.ToList().ForEach(x =>
            {
                if (x.Type == ClaimTypeName.Id)
                {
                    Id =Guid.Parse( x.Value);
                }
                if (x.Type == ClaimTypeName.UserId)
                {
                    UserId = x.Value;
                }
                if (x.Type == ClaimTypeName.UserName)
                {
                    UserName = x.Value;
                }
                if (x.Type ==ClaimTypeName.TimeZone)
                {
                    TimeZone = x.Value;
                }
            });
        }
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string BaseURI { get; set; } = null!;
        public string TimeZone { get; set; }= null!;
    }
}

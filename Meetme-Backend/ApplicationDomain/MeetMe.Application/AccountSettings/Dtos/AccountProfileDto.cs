using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.AccountSettings.Dtos
{
    public class AccountProfileDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string TimeZone { get; set; } = null!;
        public string BaseURI    { get; set; } = null!;
        public string? WelcomeText { get; set; } = null;
    }
}

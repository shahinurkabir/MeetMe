using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Core.Persistence.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserID { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string BaseURI { get; set; } = null!;
        public string TimeZone { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? WelcomeText { get; set; }
    }
}

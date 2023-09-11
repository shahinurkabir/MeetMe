using MeetMe.Application.AccountSettings.Dtos;
using MeetMe.Core.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.Calendars.Dtos
{
    public class AppointmentDto
    {
        public Appointment AppointmentDetails { get; set; } 
        public AccountProfileDto AccountProfile { get; set; } 
        public Guid EventTypeId { get; set; }
        public string EventName { get; set; } = null!;
        public string EventSlug { get; set; } = null!;
        

    }
}

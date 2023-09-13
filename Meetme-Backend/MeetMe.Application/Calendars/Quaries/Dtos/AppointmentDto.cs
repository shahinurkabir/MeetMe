using MeetMe.Application.AccountSettings.Dtos;
using MeetMe.Core.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.Calendars.Quaries.Dtos
{
    public class AppointmentDetailsDto
    {
        public Guid Id { get; set; }
        public Guid EventTypeId { get; set; }
        public string InviteeName { get; set; } = null!;
        public string InviteeEmail { get; set; } = null!;
        public string InviteeTimeZone { get; set; } = null!;
        public string? GuestEmails { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string AppointmentDateTime { get; set; } = null!;
        public string? Note { get; set; }
        public string Status { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateCancelled { get; set; }
        public string? CancellationReason { get; set; }

    }

}


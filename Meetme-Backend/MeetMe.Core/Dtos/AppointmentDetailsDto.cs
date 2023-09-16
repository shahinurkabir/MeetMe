using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Core.Dtos
{
    public class AppointmentDetailsDto
    {
        public Guid Id { get; set; }
        public Guid EventTypeId { get; set; }
        public string? EventTypeTitle { get; set; } 
        public string? EventTypeDescription { get; set; } 
        public string? EventTypeLocation { get; set; } 
        public int EventTypeDuration { get; set; } 
        public string EventTypeColor { get; set; } = null!;
        public string EventTypeTimeZone { get; set; } = null!;
        public Guid EventOwnerId { get;set ; } 
        public string EventOwnerName { get; set; } = null!;
        public string InviteeName { get; set; } = null!;
        public string InviteeEmail { get; set; } = null!;
        public string InviteeTimeZone { get; set; } = null!;
        public string? GuestEmails { get; set; }
        public DateTime StartTimeUTC { get; set; }
        public DateTime EndTimeUTC { get; set; }
        public string AppointmentDateTime { get; set; } = null!;
        public string? Note { get; set; }
        public string Status { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateCancelled { get; set; }
        public string? CancellationReason { get; set; }

    }
}

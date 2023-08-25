﻿using System.Text.Json.Serialization;

namespace MeetMe.Core.Persistence.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid EventTypeId { get; set; }
        public string InviteeName { get; set; } = null!;
        public string InviteeEmail { get; set; } = null!;
        public string? GuestEmails { get; set; }
        public DateTime StartTimeUTC { get; set; }
        public DateTime EndTimeUTC { get; set; }
        public string Status { get; set; } = null!;
        public string? Note { get; set; }

        [JsonIgnore]
        public EventType? EventType { get; set; }
    }
}
﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MeetMe.Core.Persistence.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid EventTypeId { get; set; }
        public string InviteeName { get; set; } = null!;
        public string InviteeEmail { get; set; } = null!;
        public string InviteeTimeZone { get; set; } = null!;
        public string? GuestEmails { get; set; }
        public DateTime StartTimeUTC { get; set; }
        public DateTime EndTimeUTC { get; set; }
        public string Status { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public string? Note { get; set; }
        public DateTime? DateCancelled { get; set; }
        public string? CancellationReason { get; set; }
        public Guid OwnerId { get; set; }

        [JsonIgnore]
        public EventType? EventType { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public List<AppointmentQuestionaireItem>? AppointmentQuestionaireItems { get; set; }
    }

    public class AppointmentQuestionaireItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid AppointmentId { get; set; }
        public string QuestionName { get; set; } = null!;
        public string Answer { get; set; } = null!;

        [JsonIgnore]
        public Appointment? Appointment { get; set; }

    }

}

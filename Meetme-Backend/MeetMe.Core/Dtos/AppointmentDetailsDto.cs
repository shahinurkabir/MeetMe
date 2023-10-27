using MeetMe.Core.Extensions;
using MeetMe.Core.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public Guid EventOwnerId { get; set; }
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

        public List<AppointmentQuestionaireItemDto>? Questionnaires { get; set; }

        public static AppointmentDetailsDto New(Appointment appointment, EventType eventType, User user)
        {

            var entity= new AppointmentDetailsDto
            {
                Id = appointment.Id,
                EventTypeId = appointment.EventTypeId,
                InviteeName = appointment.InviteeName,
                InviteeEmail = appointment.InviteeEmail,
                StartTimeUTC = appointment.StartTimeUTC.ToUtcIfLocal(),
                EndTimeUTC = appointment.EndTimeUTC.ToUtcIfLocal(),
                InviteeTimeZone = appointment.InviteeTimeZone,
                GuestEmails = appointment.GuestEmails,
                Note = appointment.Note,
                Status = appointment.Status.ToString(),
                DateCreated = appointment.DateCreated.ToUtcIfLocal(),
                DateCancelled = appointment.DateCancelled?.ToUtcIfLocal(),
                CancellationReason = appointment.CancellationReason,
                EventTypeTitle = eventType.Name,
                EventTypeDescription = eventType.Description,
                EventTypeLocation = eventType.Location,
                EventTypeDuration = eventType.Duration,
                EventTypeColor = eventType.EventColor,
                EventTypeTimeZone = eventType.TimeZone,
                EventOwnerId = appointment.OwnerId,
                EventOwnerName = user.UserName,
                AppointmentDateTime = appointment.InviteeTimeZone.ToAppointmentTimeRangeText(eventType.Duration, appointment.StartTimeUTC.ToUtcIfLocal()),
            };

            if (appointment.QuestionnaireContent != null)
            {
                entity.Questionnaires = JsonSerializer.Deserialize<List<AppointmentQuestionaireItemDto>>(appointment.QuestionnaireContent);
            }
            return entity;
        }


    }
}

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
        public string AppointmentTimeSlot { get; set; } = null!;
        public string AppointmentTime { get; set; } = null!;
        public string AppointmentDate { get; set; } = null!;
        public string? Note { get; set; }
        public string Status { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public DateTime? DateCancelled { get; set; }
        public string? CancellationReason { get; set; }

        public List<AppointmentQuestionaireItemDto>? Questionnaires { get; set; }
        public string DateCreatedFormattedText { get; private set; }=null!;
        public string? DateCancelledformattedText { get; private set; }
        public string? AppointmentDayDateTime { get; private set; }

        public static AppointmentDetailsDto New(Appointment appointment, EventType eventType, User user)
        {

            var entity = new AppointmentDetailsDto
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
                DateCreatedFormattedText = appointment.DateCreated.ToUtcIfLocal().ToTimeZoneFormattedText("dd MMMM yyyy",eventType.TimeZone),
                DateCancelledformattedText = appointment.DateCancelled?.ToUtcIfLocal().ToTimeZoneFormattedText("dd MMMM yyyy", eventType.TimeZone),
                CancellationReason = appointment.CancellationReason,
                EventTypeTitle = eventType.Name,
                EventTypeDescription = eventType.Description,
                EventTypeLocation = eventType.Location,
                EventTypeDuration = eventType.Duration,
                EventTypeColor = eventType.EventColor,
                EventTypeTimeZone = eventType.TimeZone,
                EventOwnerId = appointment.OwnerId,
                EventOwnerName = user.UserName,
                AppointmentTimeSlot = ToAppointmentTimeRangeText(appointment.InviteeTimeZone, eventType.Duration, appointment.StartTimeUTC.ToUtcIfLocal()),
                AppointmentDate = ToAppointmentDateText(appointment.InviteeTimeZone, eventType.Duration, appointment.StartTimeUTC.ToUtcIfLocal()),
                AppointmentTime = ToAppointmentTimeText(appointment.InviteeTimeZone, appointment.StartTimeUTC.ToUtcIfLocal()),
                AppointmentDayDateTime = appointment.DateCancelled?.ToUtcIfLocal().ToTimeZoneFormattedText("dddd, dd MMMM yyyy", eventType.TimeZone),
            };

            if (!string.IsNullOrWhiteSpace(appointment.QuestionnaireContent))
            {
                entity.Questionnaires = JsonSerializer.Deserialize<List<AppointmentQuestionaireItemDto>>(appointment.QuestionnaireContent);
            }
            return entity;
        }

        static string ToAppointmentTimeRangeText(string timeZoneName, int meetingDuration, DateTime appointmentStartTime)
        {
            var dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(appointmentStartTime, TimeZoneInfo.Utc.Id, timeZoneName);
            var startTime = dateTime.ToString("hh:mm tt");
            var endTime = dateTime.AddMinutes(meetingDuration).ToString("hh:mm tt");
            var appointmentTimeRange = $"{startTime} - {endTime}";//, {dateTime.ToString("dddd, dd MMMM yyyy")}";

            return appointmentTimeRange;
        }
        static string ToAppointmentDateText(string timeZoneName, int meetingDuration, DateTime appointmentStartTime)
        {
            var dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(appointmentStartTime, TimeZoneInfo.Utc.Id, timeZoneName);
            var appointmentTimeRange = dateTime.ToString("dddd, dd MMMM yyyy");

            return appointmentTimeRange;
        }
        static string ToAppointmentTimeText(string timeZoneName, DateTime appointmentStartTime)
        {
            var dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(appointmentStartTime, TimeZoneInfo.Utc.Id, timeZoneName);
            var startTime = dateTime.ToString("hh:mm tt");

            return startTime;
        }


    }
}

using MediatR;
using MeetMe.Application.Calendars.Quaries.Dtos;
using MeetMe.Core.Extensions;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Calendars.Quaries
{
    //create command class for get appointment  details with id type guid
    public class AppointmentDetailsQuery : IRequest<AppointmentDetailsDto>
    {
        public Guid Id { get; set; }
    }

    // create a class for handle the request
    public class AppointmentDetailsQueryHandler : IRequestHandler<AppointmentDetailsQuery, AppointmentDetailsDto>
    {
        private readonly IAppointmentsRepository appointmentsRepository;

        public AppointmentDetailsQueryHandler(IAppointmentsRepository appointmentsRepository)
        {
            this.appointmentsRepository = appointmentsRepository;
        }

        public async Task<AppointmentDetailsDto> Handle(AppointmentDetailsQuery request, CancellationToken cancellationToken)
        {
            var appointment = await appointmentsRepository.GetById(request.Id);

            var meetingDuration = (appointment.EndTimeUTC - appointment.StartTimeUTC).TotalMinutes;
            var offsetLoccal = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            var offsetInvitee = TimeZoneInfo.FindSystemTimeZoneById(appointment.InviteeTimeZone).GetUtcOffset(DateTime.UtcNow);

            var dateTime=TimeZoneInfo.ConvertTimeBySystemTimeZoneId(appointment.StartTimeUTC, TimeZoneInfo.Utc.Id, appointment.InviteeTimeZone);
            var startTime = dateTime.ToString("hh:mm tt");
            var endTime = dateTime.AddMinutes(meetingDuration).ToString("hh:mm tt");
            var appointmentDateTime =$"{startTime} - {endTime}, {dateTime.ToString("dddd, MMMM dd, yyyy")}";
            //< p > 10:00 AM - 10:30 AM, Monday, September 11, 2023 </ p >
            var appointmentDetailsDto = new AppointmentDetailsDto
            {
                Id = appointment.Id,
                EventTypeId = appointment.EventTypeId,
                InviteeName = appointment.InviteeName,
                InviteeEmail = appointment.InviteeEmail,
                InviteeTimeZone = appointment.InviteeTimeZone,
                GuestEmails = appointment.GuestEmails,
                StartTime = appointment.StartTimeUTC,
                EndTime = appointment.EndTimeUTC,
                AppointmentDateTime= appointmentDateTime,
                Note = appointment.Note,
                Status = appointment.Status.ToString(),
                DateCreated = appointment.DateCreated,
                DateCancelled = appointment.DateCancelled,
                CancellationReason = appointment.CancellationReason,
            };

            return appointmentDetailsDto;
        }
    }
}

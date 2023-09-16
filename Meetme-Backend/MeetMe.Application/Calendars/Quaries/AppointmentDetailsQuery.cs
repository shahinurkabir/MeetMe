using MediatR;
using MeetMe.Application.Calendars.Quaries.Dtos;
using MeetMe.Core.Dtos;
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
            var appointmentDetailsDto = await appointmentsRepository.GetDetailsById(request.Id);

            //var appointmentDateTime=appointmentDetails.InviteeTimeZone.ToAppointmentTimeRangeText(appointmentDetails.EventTypeDuration, appointmentDetails.StartTime, appointmentDetails.EndTime);
            //var meetingDuration = (appointmentDetails.EndTime - appointmentDetails.StartTime).TotalMinutes;

            //var dateTime=TimeZoneInfo.ConvertTimeBySystemTimeZoneId(appointmentDetails.StartTime, TimeZoneInfo.Utc.Id, appointmentDetails.InviteeTimeZone);
            //var startTime = dateTime.ToString("hh:mm tt");
            //var endTime = dateTime.AddMinutes(meetingDuration).ToString("hh:mm tt");
            //var appointmentDateTime =$"{startTime} - {endTime}, {dateTime.ToString("dddd, MMMM dd, yyyy")}";
            
            //var appointmentDetailsDto = new AppointmentDetailsDto
            //{
            //    Id = appointmentDetails.Id,
            //    EventTypeId = appointmentDetails.EventTypeId,
            //    InviteeName = appointmentDetails.InviteeName,
            //    InviteeEmail = appointmentDetails.InviteeEmail,
            //    InviteeTimeZone = appointmentDetails.InviteeTimeZone,
            //    GuestEmails = appointmentDetails.GuestEmails,
            //    StartTime = appointmentDetails.StartTimeUTC,
            //    EndTime = appointmentDetails.EndTimeUTC,
            //    AppointmentDateTime= appointmentDateTime,
            //    Note = appointmentDetails.Note,
            //    Status = appointmentDetails.Status.ToString(),
            //    DateCreated = appointmentDetails.DateCreated,
            //    DateCancelled = appointmentDetails.DateCancelled,
            //    CancellationReason = appointmentDetails.CancellationReason,
            //};

            return appointmentDetailsDto;
        }
    }
}

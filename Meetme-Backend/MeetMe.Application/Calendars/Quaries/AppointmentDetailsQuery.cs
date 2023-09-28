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
        private readonly IAppointmentRepository _appointmentsRepository;

        public AppointmentDetailsQueryHandler(IAppointmentRepository appointmentsRepository)
        {
            _appointmentsRepository = appointmentsRepository;
        }

        public async Task<AppointmentDetailsDto> Handle(AppointmentDetailsQuery request, CancellationToken cancellationToken)
        {
            var appointmentDetailsDto = await _appointmentsRepository.GetAppointmentDetails(request.Id);

            return appointmentDetailsDto;
        }
    }
}

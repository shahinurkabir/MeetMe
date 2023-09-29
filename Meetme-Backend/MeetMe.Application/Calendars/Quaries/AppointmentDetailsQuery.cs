using MediatR;
using MeetMe.Application.Calendars.Quaries.Dtos;
using MeetMe.Core.Dtos;
using MeetMe.Core.Extensions;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Calendars.Quaries
{
    public class AppointmentDetailsQuery : IRequest<AppointmentDetailsDto>
    {
        public Guid Id { get; set; }
    }

    public class AppointmentDetailsQueryHandler : IRequestHandler<AppointmentDetailsQuery, AppointmentDetailsDto>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public AppointmentDetailsQueryHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }

        public async Task<AppointmentDetailsDto> Handle(AppointmentDetailsQuery request, CancellationToken cancellationToken)
        {
            var appointmentDetailsDto = await persistenceProvider.GetAppointmentDetails(request.Id);

            return appointmentDetailsDto;
        }
    }
}

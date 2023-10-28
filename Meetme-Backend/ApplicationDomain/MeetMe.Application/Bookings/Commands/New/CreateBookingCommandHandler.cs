using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;

namespace MeetMe.Application.Bookings.Commands.New
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
    {
        private readonly IPersistenceProvider persistenseProvider;
        private readonly IUserInfo applicationUserInfo;

        public CreateBookingCommandHandler(IPersistenceProvider persistenseProvider, IUserInfo applicationUserInfo)
        {
            this.persistenseProvider = persistenseProvider;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // Validate requred fields
            // validate that time slot selected is valid
            // validate available in calendars - already intregated into system

            var eventTypeInformation = await persistenseProvider.GetEventTypeById(request.EventTypeId);

            var startTimeUTC = request.StartTime.ToUniversalTime();
            var endTimeUTC = startTimeUTC.AddMinutes(eventTypeInformation.Duration);

            var newBookingId = Guid.NewGuid();

            var bookingInformation = new BookingInformation
            {
                Id = newBookingId,
                EventTypeId = eventTypeInformation.Id,
                Name = eventTypeInformation.Name,
                InviteeEmail = request.Email,
                InviteeFullName = request.FullName,
                InviteeTimeZoneId = request.TimeZoneId,
                GuestEmails = request.GuestEmails,
                StartTime = startTimeUTC,
                EndTime = endTimeUTC,
                Remarks = request.Remarks,
                CreatedBy = applicationUserInfo.UserId,
                DateCreated = DateTime.UtcNow
            };

            await persistenseProvider.AddNewBooking(bookingInformation);

            return await Task.FromResult(newBookingId);


        }

        public bool ValidateAvailableTimeSlot(CreateBookingCommand createBookingCommand) => true;
    }
}

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;

namespace MeetMe.Application.Bookings.Commands.Cancel
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, bool>
    {
        private readonly IPersistenceProvider persistenceProvider;
        private readonly IUserInfo applicationUserInfo;

        public CancelBookingCommandHandler(IPersistenceProvider persistenceProvider, Core.Interface.IUserInfo applicationUserInfo)
        {
            this.persistenceProvider = persistenceProvider;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<bool> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var bookingModel = await persistenceProvider.GetBookingById(request.BookingId);

            if (bookingModel == null) throw new Exception("Booking cancelling fail.Invalid booking provided");

            if (bookingModel.Cancel) throw new Exception("This booking is already cancelled");

            bookingModel.CancelledBy = applicationUserInfo.UserId;
            bookingModel.Cancel = true;
            bookingModel.CancelRemarks = request.CancelRemarks;
            bookingModel.DateCancelled = DateTime.UtcNow;

            await persistenceProvider.UpdateBooking(bookingModel);

            return await Task.FromResult(true);
        }
    }
}

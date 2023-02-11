using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Application.Bookings.Commands.Cancel
{
    public class CancelBookingCommand : IRequest<bool>
    {
        public Guid BookingId { get; set; }
        public string CancelRemarks { get; set; }
    }
}

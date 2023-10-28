using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Interface.Persistence
{
    public interface IBookingRepository
    {
        Task<BookingInformation> GetBookingById(Guid bookingId);
        Task<bool> AddNewBooking(BookingInformation bookingInformation);
        Task UpdateBooking(BookingInformation bookingModel);
    }
}

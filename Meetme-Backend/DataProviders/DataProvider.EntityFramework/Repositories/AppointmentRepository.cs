using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;

namespace DataProvider.EntityFramework.Repositories
{
    public class AppointmentRepository : IAppointmentsRepository
    {
        private readonly BookingDbContext bookingDbContext;

        public AppointmentRepository( BookingDbContext bookingDbContext)
        {
            this.bookingDbContext = bookingDbContext;
        }
        public async Task AddNewAppointment(CalendarAppointment appointment)
        {
            await bookingDbContext.Set<CalendarAppointment>().AddAsync(appointment);

            await bookingDbContext.SaveChangesAsync();
        }

        public Task<CalendarAppointment> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

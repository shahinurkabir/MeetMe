using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.EntityFramework.Repositories
{
    public class AppointmentRepository : IAppointmentsRepository
    {
        private readonly BookingDbContext bookingDbContext;

        public AppointmentRepository(BookingDbContext bookingDbContext)
        {
            this.bookingDbContext = bookingDbContext;
        }
        public async Task AddAppointment(Appointment appointment)
        {
            await bookingDbContext.Set<Appointment>().AddAsync(appointment);

            await bookingDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAppointment(Guid id)
        {
            await bookingDbContext.Set<Appointment>().Where(x => x.Id == id).ExecuteDeleteAsync();
            return await bookingDbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Appointment>> GetAppointmentsByDateRange(Guid eventTypeId, DateTime startDateUTC, DateTime endDateUTC)
        {
            var result = await bookingDbContext.Set<Appointment>()
                .Where(x => x.EventTypeId == eventTypeId && x.StartTimeUTC >= startDateUTC && x.EndTimeUTC <= endDateUTC)
                .ToListAsync();

            return result;
        }

        public async Task<Appointment> GetById(Guid id)
        {
            return await bookingDbContext.Set<Appointment>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsTimeConflicting(Guid eventTypeId, DateTime startDateUTC, DateTime endDateUTC)
        {
            return await bookingDbContext.Set<Appointment>()
                .AnyAsync(x => x.EventTypeId == eventTypeId && x.StartTimeUTC >= startDateUTC && x.EndTimeUTC <= endDateUTC);
        }

        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            bookingDbContext.Set<Appointment>().Update(appointment);
            return await bookingDbContext.SaveChangesAsync() > 0;
        }
    }
}

using MeetMe.Core.Dtos;
using MeetMe.Core.Extensions;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.EntityFramework.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly BookingDbContext _bookingDbContext;

        public AppointmentRepository(BookingDbContext bookingDbContext)
        {
            this._bookingDbContext = bookingDbContext;
        }
        public async Task<bool> AddAppointment(Appointment appointment)
        {
            await _bookingDbContext.Set<Appointment>().AddAsync(appointment);

            await _bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAppointment(Guid id)
        {
            await _bookingDbContext.Set<Appointment>().Where(x => x.Id == id).ExecuteDeleteAsync();
            return await _bookingDbContext.SaveChangesAsync() > 0;
        }

        public async Task<Appointment> GetAppointment(Guid id)
        {
            return await _bookingDbContext.Set<Appointment>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsTimeBooked(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            return await _bookingDbContext.Set<Appointment>()
                .AnyAsync(x => x.EventTypeId == eventTypeId && x.StartTimeUTC >= startDateUTC && x.EndTimeUTC <= endDateUTC);
        }

        public async Task<bool> UpdateAppointment(Appointment appointment)
        {
            _bookingDbContext.Set<Appointment>().Update(appointment);
            return await _bookingDbContext.SaveChangesAsync() > 0;
        }

        public async Task<AppointmentDetailsDto?> GetAppointmentDetails(Guid id)
        {
            var result = await _bookingDbContext.Set<Appointment>()
                 .Where(x => x.Id == id)
                 .Include(x => x.EventType)
                 .ThenInclude(x => x.User)
                 .Select(x => AppointmentDetailsDto.New(x, x.EventType!, x.EventType!.User))
                 .FirstOrDefaultAsync();

            return result;

        }

        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsByUserId(Guid userId)
        {
            var result = await _bookingDbContext.Set<Appointment>()
                  .Include(x => x.EventType)
                  .ThenInclude(x => x.User)
                  .Where(x => x.EventType.User.Id == userId)
                  .Select(x => AppointmentDetailsDto.New(x, x.EventType!, x.EventType!.User))
                  .ToListAsync();

            return result;
        }
        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsOfEventTypeByDateRange(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            var result = await _bookingDbContext.Set<Appointment>()
                  .Include(x => x.EventType)
                  .ThenInclude(x => x.User)
                  .Where(x => x.EventTypeId == eventTypeId && x.StartTimeUTC >= startDateUTC && x.EndTimeUTC <= endDateUTC)
                  .Select(x => AppointmentDetailsDto.New(x,x.EventType!,x.EventType!.User))
                  .ToListAsync();

            return result;
        }
       


    }
}

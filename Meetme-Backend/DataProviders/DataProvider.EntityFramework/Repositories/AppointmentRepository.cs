using MeetMe.Core.Dtos;
using MeetMe.Core.Extensions;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.EntityFramework.Repositories
{
    public class AppointmentRepository : IAppointmentsRepository
    {
        private readonly BookingDbContext _bookingDbContext;

        public AppointmentRepository(BookingDbContext bookingDbContext)
        {
            this._bookingDbContext = bookingDbContext;
        }
        public async Task AddAppointment(Appointment appointment)
        {
            await _bookingDbContext.Set<Appointment>().AddAsync(appointment);

            await _bookingDbContext.SaveChangesAsync();
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
                 .Select(x => ToAppointmentDto(x))
                 .FirstOrDefaultAsync();

            return result;

        }

        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsByUserId(Guid userId)
        {
            var result = await _bookingDbContext.Set<Appointment>()
                  .Include(x => x.EventType)
                  .ThenInclude(x => x.User)
                  .Where(x => x.EventType.User.Id == userId)
                  .Select(x => ToAppointmentDto(x))
                  .ToListAsync();

            return result;
        }
        public async Task<List<AppointmentDetailsDto>?> GetAppointmentsOfEventTypeByDateRange(Guid eventTypeId, DateTimeOffset startDateUTC, DateTimeOffset endDateUTC)
        {
            var result = await _bookingDbContext.Set<Appointment>()
                  .Include(x => x.EventType)
                  .ThenInclude(x => x.User)
                  .Where(x => x.EventTypeId == eventTypeId && x.StartTimeUTC >= startDateUTC && x.EndTimeUTC <= endDateUTC)
                  .Select(x => ToAppointmentDto(x))
                  .ToListAsync();

            return result;
        }
        private static AppointmentDetailsDto ToAppointmentDto(Appointment x)
        {

            var dto = new AppointmentDetailsDto
            {
                Id = x.Id,
                EventTypeId = x.EventTypeId,
                InviteeName = x.InviteeName,
                InviteeEmail = x.InviteeEmail,
                StartTimeUTC = x.StartTimeUTC,
                EndTimeUTC = x.EndTimeUTC,
                InviteeTimeZone = x.InviteeTimeZone,
                GuestEmails = x.GuestEmails,
                Note = x.Note,
                Status = x.Status.ToString(),
                DateCreated = x.DateCreated,
                DateCancelled = x.DateCancelled,
                CancellationReason = x.CancellationReason,
                EventTypeTitle = x.EventType.Name,
                EventTypeDescription = x.EventType.Description,
                EventTypeLocation = x.EventType.Location,
                EventTypeDuration = x.EventType.Duration,
                EventTypeColor = x.EventType.EventColor,
                EventTypeTimeZone = x.EventType.TimeZone,
                EventOwnerId = x.EventType.OwnerId,
                EventOwnerName = x.EventType.User.UserName,
                AppointmentDateTime = x.InviteeTimeZone.ToAppointmentTimeRangeText(x.EventType.Duration, x.StartTimeUTC),
            };

            return dto;
        }


    }
}

using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.EntityFramework.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly BookingDbContext _bookingDbContext;

        public AvailabilityRepository(BookingDbContext bookingDbContext)
        {
            _bookingDbContext = bookingDbContext;
        }
        public async Task<bool> AddAvailability(Availability availability)
        {
            await _bookingDbContext.AddAsync(availability);
            _bookingDbContext.SaveChanges();

            return true;

        }

        public async Task<bool> UpdateAvailability(Availability availability)
        {
            var listScheduleLineItem = await _bookingDbContext.Set<AvailabilityDetail>()
                .Where(e => e.AvailabilityId == availability.Id)
                .ToListAsync();

            _bookingDbContext.RemoveRange(listScheduleLineItem);

            _bookingDbContext.Update(availability);

            // update time schedule of all event type availability those are related to this schedule rule
            var listEventType = await _bookingDbContext.Set<EventType>()
                .Where(e => e.AvailabilityId == availability.Id)
                .Include(e => e.EventTypeAvailabilityDetails)
                .ToListAsync();

            foreach (var eventTypeItem in listEventType)
            {
                eventTypeItem.EventTypeAvailabilityDetails.Clear();
                eventTypeItem.EventTypeAvailabilityDetails.AddRange(availability.Details.Select(e => new EventTypeAvailabilityDetail
                {
                    EventTypeId = eventTypeItem.Id,
                    DayType = e.DayType,
                    Value = e.Value,
                    From = e.From,
                    To = e.To,
                    StepId = e.StepId,
                }));
            }

            _bookingDbContext.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAvailability(Availability availability)
        {
            availability.IsDeleted = true;

            // detach all event type availability those are related from this schedule rule
            var listEventType = await _bookingDbContext.Set<EventType>()
                .Where(e => e.AvailabilityId == availability.Id)
                .ToListAsync();

            foreach (var eventTypeItem in listEventType)
            {
                eventTypeItem.AvailabilityId = null;
            }

            await _bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Availability>> GetListByUserId(Guid userId)
        {
            var list = await GetAvailabilityList(userId);

            return list;
        }

        public async Task<Availability> GetAvailability(Guid ruleId)
        {
            var scheduleRule = await _bookingDbContext.Set<Availability>()
                .Include(e => e.Details)
                .Where(e => e.Id == ruleId)
                .FirstOrDefaultAsync();

            return scheduleRule;
        }

        public async Task<bool> UpdateAvailabilityName(Guid id, string nameToUpdate)
        {
            var entity = await _bookingDbContext.Set<Availability>()
                .FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            entity.Name = nameToUpdate;

            await _bookingDbContext.SaveChangesAsync();

            return true;

        }
        public async Task<bool> SetDefaultAvailability(Guid id, Guid userId)
        {
            var listAvailabilityForUser = await GetAvailabilityList(userId);

            var entity = listAvailabilityForUser.FirstOrDefault(e => e.Id == id);

            if (entity == null)

            {
                return false;
            }

            //Reset all availability to non-default
            listAvailabilityForUser.ForEach(e => e.IsDefault = false);

            entity.IsDefault = true;

            await _bookingDbContext.SaveChangesAsync();

            return true;

        }

        private async Task<List<Availability>> GetAvailabilityList(Guid userId)
        {
            var list = await _bookingDbContext.Set<Availability>()
                .Include(e => e.Details)
                .Where(e => e.OwnerId == userId)
                .ToListAsync();

            return list;
        }


    }
}

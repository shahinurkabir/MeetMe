using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.EntityFramework.Repositories
{
    public class EventTypeRepository : IEventTypeRepository
    {
        private readonly MeetMeDbContext _bookingDbContext;

        public EventTypeRepository(MeetMeDbContext bookingDbContext)
        {
            _bookingDbContext = bookingDbContext;
        }

        public async Task<bool> AddNewEventType(EventType eventTypeInfo)
        {
            await _bookingDbContext.AddAsync(eventTypeInfo);

            await _bookingDbContext.SaveChangesAsync();

            return true;

        }

        public async Task<EventType> GetEventTypeById(Guid eventTypeId)
        {
            var entity = await _bookingDbContext.Set<EventType>()
                .Where(e => e.Id == eventTypeId)
                .Include(e => e.EventTypeAvailabilityDetails)
                .Include(e => e.Questions)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<EventType> GetEventTypeBySlug(string slug)
        {
            var entity = await _bookingDbContext.Set<EventType>()
                .Where(e => e.Slug == slug)
                .Include(e => e.EventTypeAvailabilityDetails)
                .Include(e => e.Questions)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<bool> UpdateEventType(EventType eventTypeInfo)
        {
            _bookingDbContext.Update(eventTypeInfo);

            await _bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateEventAvailability(EventType eventTypeInfo, List<EventTypeAvailabilityDetail> scheduleDetails)
        {
            var eventTypeId = eventTypeInfo.Id;

            var existingAvailabilityDetails = await _bookingDbContext.Set<EventTypeAvailabilityDetail>()
              .Where(e => e.EventTypeId == eventTypeId)
              .ToListAsync();

            if (existingAvailabilityDetails.Any())
            {
                _bookingDbContext.RemoveRange(existingAvailabilityDetails);

            }
            await _bookingDbContext.AddRangeAsync(scheduleDetails);

            _bookingDbContext.Update(eventTypeInfo);

            await _bookingDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<EventType>> GetEventTypeListByUserId(Guid userId)
        {
            var result = await _bookingDbContext.Set<EventType>()
                .Where(e => e.OwnerId == userId)
                .ToListAsync();

            return result;
        }

       
    }
}

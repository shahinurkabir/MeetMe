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
        private readonly BookingDbContext bookingDbContext;

        public EventTypeRepository(BookingDbContext bookingDbContext)
        {
            this.bookingDbContext = bookingDbContext;
        }

        public async Task AddNewEventType(EventType eventTypeInfo)
        {
            await bookingDbContext.AddAsync(eventTypeInfo);

            await bookingDbContext.SaveChangesAsync();

        }

        public async Task<EventType> GetEventTypeById(Guid eventTypeId)
        {
            var entity = await bookingDbContext.Set<EventType>()
                .Where(e => e.Id == eventTypeId)
                .Include(e => e.EventTypeAvailabilityDetails)
                .Include(e => e.Questions).FirstAsync();

            return entity;
        }

        public async Task<List<EventType>> GetEventTypeList()
        {

            var list = await bookingDbContext.Set<EventType>().ToListAsync();

            return list;

        }
        public async Task UpdateEventType(EventType eventTypeInfo)
        {
            bookingDbContext.Update(eventTypeInfo);

            await bookingDbContext.SaveChangesAsync();
        }

        //public async Task<bool> SlugUsedYN(string slug, Guid userId)
        //{
        //    var result = await bookingDbContext.Set<EventType>()
        //        .Where(e => e.OwnerId == userId && e.Slug == slug)
        //        .CountAsync();

        //    return result > 0;
        //}

        public async Task<List<EventType>> GetEventTypeListByUserId(Guid userId)
        {
            var result = await bookingDbContext.Set<EventType>()
                .Where(e => e.OwnerId == userId)
                .ToListAsync();

            return result;
        }


    }
}

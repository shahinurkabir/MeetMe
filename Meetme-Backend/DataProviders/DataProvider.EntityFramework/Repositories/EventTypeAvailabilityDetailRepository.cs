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
    public class EventTypeAvailabilityDetailRepository : IEventTypeAvailabilityDetailRepository
    {
        private readonly BookingDbContext bookingDbContext;

        public EventTypeAvailabilityDetailRepository(BookingDbContext bookingDbContext)
        {
            this.bookingDbContext = bookingDbContext;
        }

        public async Task<List<EventTypeAvailabilityDetail>> GetEventTypeAvailabilityDetailByEventId(Guid eventTypeId)
        {
            var entity = await bookingDbContext.Set<EventTypeAvailabilityDetail>().Where(e => e.EventTypeId == eventTypeId).ToListAsync();

            return entity;
        }

        public async Task InsertItems(List<EventTypeAvailabilityDetail> itemsToInsert)
        {
            await bookingDbContext.Set<EventTypeAvailabilityDetail>().AddRangeAsync(itemsToInsert);

            await bookingDbContext.SaveChangesAsync();

        }

        public async Task RemoveItems(List<EventTypeAvailabilityDetail> itemsToRemove)
        {
            bookingDbContext.Set<EventTypeAvailabilityDetail>().RemoveRange(itemsToRemove);

            await bookingDbContext.SaveChangesAsync();
        }
    }
}

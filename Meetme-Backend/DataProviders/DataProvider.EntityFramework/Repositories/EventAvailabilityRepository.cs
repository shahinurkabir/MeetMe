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
    //public class EventAvailabilityRepository : IEventAvailabilityRepository
    //{
    //    private readonly BookingDbContext bookingDbContext;

    //    public EventAvailabilityRepository(BookingDbContext bookingDbContext)
    //    {
    //        this.bookingDbContext = bookingDbContext;
    //    }

    //    public async Task<EventTypeAvailability> GetEventAvailabilityById(Guid eventTypeId)
    //    {
    //        var result = await bookingDbContext.Set<EventTypeAvailability>()
    //            .Include(e => e.AvailabilityDetails)
    //            .FirstOrDefaultAsync(e => e.Id == eventTypeId)
    //                            ;

    //        return result;
    //    }

    //    public async Task ResetAvailability(EventTypeAvailability eventTypeAvailability)
    //    {
    //        var eventTypeId = eventTypeAvailability.Id;
            
    //        var entity = await bookingDbContext.Set<EventTypeAvailability>()
    //            .Include(e => e.AvailabilityDetails)
    //            .FirstOrDefaultAsync(e => e.Id == eventTypeId);

    //        if (entity != null)
    //        {
    //            bookingDbContext.RemoveRange(entity.AvailabilityDetails);

    //            bookingDbContext.Remove(entity);
    //        }

    //        await bookingDbContext.AddAsync(eventTypeAvailability);

    //        await bookingDbContext.SaveChangesAsync();

    //    }

    //}
}

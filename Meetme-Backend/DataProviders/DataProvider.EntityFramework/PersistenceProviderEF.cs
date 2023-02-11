using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace DataProvider.EntityFramework
{
    public partial class PersistenceProviderEF : IPersistenceProvider
    {
        private BookingDbContext bookingDbContext;

        public PersistenceProviderEF(BookingDbContext bookingDbContext)
        {
            this.bookingDbContext = bookingDbContext;
            bookingDbContext.Database.EnsureCreated();
        }

        public Task<bool> AddNewBooking(BookingInformation bookingInformation)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddNewEventType(EventType eventTypeInfo)
        {
            await bookingDbContext.AddAsync(eventTypeInfo);

            return true;
        }

        public async Task<bool> AddNewEventTypeScheduleRule(EventTypeScheduleInfo scheduleRule)
        {
            await bookingDbContext.AddAsync(scheduleRule);
            return true;
        }

        public Task<BookingInformation> GetBookingById(Guid bookingId)
        {
            throw new NotImplementedException();
        }

        public async Task<EventType> GetEventTypeById(Guid eventTypeId)
        {
            var entity = await bookingDbContext.Set<EventType>().FindAsync(eventTypeId);
            return entity;
        }

        public async Task<List<EventType>> GetEventTypeList()
        {

            var list = await bookingDbContext.Set<EventType>().ToListAsync();

            return list;

        }

        public Task<EventTypeScheduleInfo> GetEventTypeScheduleById(Guid eventTypeId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBooking(BookingInformation bookingModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEventType(EventType eventTypeInfo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateSchedule(EventTypeScheduleInfo scheduleRule)
        {
            throw new NotImplementedException();
        }

        //public async Task AddNewEventAvailability(EventTypeAvailability eventTypeAvailability)
        //{
        //    await bookingDbContext.Set<EventTypeAvailability>().AddAsync(eventTypeAvailability);

        //}
        //public async Task<EventTypeAvailability> GetEventAvailabilityById(Guid eventTypeId)
        //{
        //    var result = await bookingDbContext.Set<EventTypeAvailability>().FindAsync(eventTypeId);

        //    return result;
        //}

        //public Task UpdateEventAvailability(EventTypeAvailability eventTypeAvailability)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task DeleteEventAvailability(Guid eventTypeId)
        //{
        //    var entity = await bookingDbContext.Set<EventTypeAvailability>().FindAsync(eventTypeId);

        //    if (entity != null)
        //    {
        //        var listDetails = await bookingDbContext.Set<EventTypeAvailabilityDetail>()
        //            .Where(e => e.AvailabilityId == eventTypeId).ToListAsync();

        //        bookingDbContext.RemoveRange(listDetails);

        //        bookingDbContext.Remove(entity);
        //    }

            

        //}

        #region UnitOfWork
        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken)
        {
            var result = await bookingDbContext.SaveChangesAsync(cancellationToken);

            return result;
        }

        



        #endregion
    }
}

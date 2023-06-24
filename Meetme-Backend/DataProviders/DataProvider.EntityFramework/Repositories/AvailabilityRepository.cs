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
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly BookingDbContext bookingDbContext;

        public AvailabilityRepository(BookingDbContext bookingDbContext)
        {
            this.bookingDbContext = bookingDbContext;
        }
        public async Task<bool> AddSchedule(Availability scheduleRule)
        {
            await bookingDbContext.AddAsync(scheduleRule);
            bookingDbContext.SaveChanges();

            return true;

        }

        public async Task<bool> UpdateSchedule(Availability scheduleRule)
        {
            var listScheduleLineItem = await bookingDbContext.Set<AvailabilityDetail>().Where(e => e.AvailabilityId == scheduleRule.Id).ToListAsync();

            bookingDbContext.RemoveRange(listScheduleLineItem);

            bookingDbContext.Update(scheduleRule);

            // update time schedule of all event type availability those are related to this schedule rule
            var listEventType = await bookingDbContext.Set<EventType>().Where(e => e.AvailabilityId == scheduleRule.Id).Include(e => e.EventTypeAvailabilityDetails).ToListAsync();

            foreach (var eventTypeItem in listEventType)
            {
                eventTypeItem.EventTypeAvailabilityDetails.Clear();
                eventTypeItem.EventTypeAvailabilityDetails.AddRange(scheduleRule.Details.Select(e => new EventTypeAvailabilityDetail
                {
                    EventTypeId = eventTypeItem.Id,
                    DayType = e.DayType,
                    Value = e.Value,
                    From = e.From,
                    To = e.To,
                    StepId = e.StepId,
                }));
            }

            bookingDbContext.SaveChanges();

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteSchedule(Availability entity)
        {
            entity.IsDeleted = true;

            // detach all event type availability those are related to this schedule rule
            var listEventType = await bookingDbContext.Set<EventType>().Where(e => e.AvailabilityId == entity.Id).ToListAsync();
            foreach (var eventTypeItem in listEventType) { eventTypeItem.AvailabilityId = null;}
         
            await bookingDbContext.SaveChangesAsync();

            return Task.FromResult(true).Result;
        }

        public async Task<List<Availability>> GetListByUserId(Guid userId)
        {
            var list = await GetAvailabilityList(userId);

            return list;
        }

        public async Task<Availability> GetById(Guid ruleId)
        {
            var scheduleRule = await bookingDbContext.Set<Availability>()
                .Include(e => e.Details)
                .Where(e => e.Id == ruleId).FirstOrDefaultAsync();

            return scheduleRule;
        }

        public async Task<bool> EditName(Guid id, string nameToUpdate)
        {
            var entity = await bookingDbContext.Set<Availability>().FindAsync(id);

            if (entity == null) return false;

            entity.Name = nameToUpdate;

            await bookingDbContext.SaveChangesAsync();

            return true;

        }
        public async Task<bool> SetDefault(Guid id, Guid userId)
        {
            var listAvailabilityForUser = await GetAvailabilityList(userId);

            var entity = listAvailabilityForUser.FirstOrDefault(e => e.Id == id);

            if (entity == null) return false;

            //Reset 
            listAvailabilityForUser.ForEach(e => e.IsDefault = false);

            entity.IsDefault = true;

            await bookingDbContext.SaveChangesAsync();

            return true;

        }

        private async Task<List<Availability>> GetAvailabilityList(Guid userId)
        {
            var list = await bookingDbContext.Set<Availability>()
                .Include(e => e.Details)
                .Where(e => e.OwnerId == userId).ToListAsync();

            return list;
        }

        
    }
}

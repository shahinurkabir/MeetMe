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
            bookingDbContext.SaveChanges();
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteSchedule(Guid ruleId)
        {
            var entity = await bookingDbContext.Set<Availability>().FindAsync(ruleId);
            var listScheduleLineItem = await bookingDbContext.Set<AvailabilityDetail>().Where(e => e.AvailabilityId == ruleId).ToListAsync();

            bookingDbContext.RemoveRange(listScheduleLineItem);

            bookingDbContext.Remove(entity);
            bookingDbContext.SaveChanges();

            return await Task.FromResult(true);
        }

        public async Task<List<Availability>> GetScheduleListByUserId(Guid userId)
        {
            var list = await bookingDbContext.Set<Availability>()
                .Include(e => e.Details)
                .Where(e => e.OwnerId == userId).ToListAsync();

            return list;
        }

        public async Task<Availability> GetScheduleById(Guid ruleId)
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
    }
}

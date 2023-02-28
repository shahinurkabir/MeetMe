using MeetMe.Core.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.EntityFramework
{
    public partial class PersistenceProviderEF
    {
        //public async Task<bool> AddSchedule(Availability scheduleRule)
        //{
        //    await bookingDbContext.AddAsync(scheduleRule);

        //    return true;

        //}

        //public async Task<bool> UpdateSchedule(Availability scheduleRule)
        //{
        //    bookingDbContext.Update(scheduleRule);

        //    return await Task.FromResult(true);
        //}

        //public async Task<bool> DeleteSchedule(Guid ruleId)
        //{
        //    var entity = await bookingDbContext.Set<Availability>().FindAsync(ruleId);
        //    var listScheduleLineItem = await bookingDbContext.Set<AvailabilityDetail>().Where(e => e.RuleId == ruleId).ToListAsync();

        //    bookingDbContext.RemoveRange(listScheduleLineItem);

        //    bookingDbContext.Remove(entity);

        //    return await Task.FromResult(true);
        //}

        //public async Task<List<Availability>> GetScheduleListByUserId(Guid userId)
        //{
        //    var list = await bookingDbContext.Set<Availability>()
        //        .Include(e => e.Details)
        //        .Where(e => e.OwnerId == userId).ToListAsync();

        //    return list;
        //}

        //public async Task<Availability> GetScheduleById(Guid ruleId)
        //{
        //    var scheduleRule = await bookingDbContext.Set<Availability>()
        //        .Include(e => e.Details)
        //        .Where(e => e.Id == ruleId).FirstOrDefaultAsync();

        //    return scheduleRule;
        //}

    }
}

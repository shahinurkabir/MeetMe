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
    //public class TimeZoneDataRepository : ITimeZoneDataRepository
    //{
    //    private readonly BookingDbContext bookingDbContext;

    //    public TimeZoneDataRepository(BookingDbContext bookingDbContext)
    //    {
    //        this.bookingDbContext = bookingDbContext;
    //    }
    //    public async Task<List<TimeZoneData>> GetTimeZoneList()
    //    {
    //        return await bookingDbContext.Set<TimeZoneData>().ToListAsync();
    //    }
    //    public async Task<TimeZoneData?> TimeZoneDataById(int id)
    //    {
    //        var entity = await bookingDbContext.Set<TimeZoneData>()
    //             .Where(e => e.Id == id).FirstOrDefaultAsync();

    //        return entity;
    //    }

    //    public async Task<TimeZoneData?> GetTimeZoneByName(string name)
    //    {
    //        var entity = await bookingDbContext.Set<TimeZoneData>()
    //             .FirstOrDefaultAsync(e => e.Name == name);

    //        return entity;
    //    }
    //}
}

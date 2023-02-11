using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface.Caching;

namespace MeetMe.Application.Services
{
    public interface IEventTypeService
    {
        Task<List<DateTime>> GetTimeSlots(Guid eventTypeId, int year, int month);
    }
    public class EventTypeService : IEventTypeService
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly ICacheService cacheService;

        public EventTypeService(IEventTypeRepository eventTypeRepository, ICacheService cacheService)
        {
            this.eventTypeRepository = eventTypeRepository;
            this.cacheService = cacheService;
        }


        public async Task<List<DateTime>> GetTimeSlots(Guid eventTypeId, int year, int month)
        {
            var cacheKey = $"CalnedarTimeSpot-{eventTypeId}{year}{month}";

            var fromDate = DateTime.Parse($"{year}-{month}-01");
            var toDate = fromDate.AddMonths(1).AddDays(-1);

            var slots= new List<DateTime>();// await cacheService.GetOrAdd( cacheKey,() => GenerateTimeSlot(eventTypeId, fromDate, toDate), aliveForSeconds:3600);

            return slots;

        }
        
        //private async Task<List<DateTime>> GenerateTimeSlot(Guid eventTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    var calendarInfo = await eventTypeRepository.GetEventTypeById(eventTypeId);
        //    var scheduleInfo = await persistenceProvider.GetEventTypeScheduleById(eventTypeId);

        //    var tempDate = fromDate;

        //    var slotTimes = new List<DateTime>();

        //    while (tempDate <= toDate)
        //    {
        //        var dates = MeetMe.Application.Util.ApplicationUtil.GenerateTimeSpotsInDate(tempDate.Date, 0, 0, calendarInfo.Duration, calendarInfo.BufferTimeBefore, calendarInfo.BufferTimeAfter, scheduleInfo.WeeklyTimeSchedule);

        //        slotTimes.AddRange(dates);

        //        tempDate = tempDate.AddDays(1);
        //    }

        //    return slotTimes;
        //}
    }
}

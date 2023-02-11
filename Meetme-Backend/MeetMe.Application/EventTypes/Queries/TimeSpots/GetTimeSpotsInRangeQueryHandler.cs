using MediatR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MeetMe.Utility.Extensions;
using MeetMe.Application.Services;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Queries.TimeSpots
{
    public class GetTimeSpotsInRangeQueryHandler : IRequestHandler<GetTimeSpotsInRangeQuery, TimeSpotsInRangeResponse>
    {
        private readonly IPersistenceProvider persistenceProvider;
        private readonly IEventTypeService eventTypeService;

        public GetTimeSpotsInRangeQueryHandler(IPersistenceProvider persistenceProvider, IEventTypeService eventTypeService)
        {
            this.persistenceProvider = persistenceProvider;
            this.eventTypeService = eventTypeService;
        }
        public async Task<TimeSpotsInRangeResponse> Handle(GetTimeSpotsInRangeQuery request, CancellationToken cancellationToken)
        {
            var timeSlotResponse = new TimeSpotsInRangeResponse();

            var calendarInfo = await persistenceProvider.GetEventTypeById(request.CalendarId);
            var scheduleInfo = await persistenceProvider.GetEventTypeScheduleById(request.CalendarId);

            var timeZoneInfo_Calendar = TimeZoneInfo.FindSystemTimeZoneById("calendarInfo.TimeZoneId");
            var timeZoneInfo_User = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZoneId);

            var localTime_Server = DateTime.Now;
            var localTime_Server_UTC = DateTimeOffset.UtcNow;

            var localTime_Calendar = localTime_Server.ToTimeZoneTime(timeZoneInfo_Calendar);
            var localTime_User = localTime_Server.ToTimeZoneTime(timeZoneInfo_User);
            var locatTime_User_UTC = localTime_User.ToUniversalTime();

            var offsetDiffInMinutes = timeZoneInfo_User.BaseUtcOffset.TotalMinutes;

            var fromDate_UTC = new DateTimeOffset(request.FromDate, timeZoneInfo_User.BaseUtcOffset).ToUniversalTime();

            var toDate_UTC = new DateTimeOffset(request.ToDate.Add(TimeSpan.Parse("23:59:59")), timeZoneInfo_User.BaseUtcOffset).ToUniversalTime();

            if (fromDate_UTC < locatTime_User_UTC)
                fromDate_UTC = locatTime_User_UTC;

            var timeSlots = new List<DateTime>();

            var slots = await eventTypeService.GetTimeSlots(request.CalendarId, request.FromDate.Year, request.FromDate.Month);

            timeSlots.AddRange(slots);

            if (request.FromDate.Month != request.ToDate.Month)
            {
                slots = await eventTypeService.GetTimeSlots(request.CalendarId, request.FromDate.Year, request.FromDate.Month);

                timeSlots.AddRange(slots);
            }

            var timeSlotsInDateRange = timeSlots.Where(e => e >= fromDate_UTC && e < toDate_UTC).ToList();

            var timeSlotsConvertedTimeZone = timeSlotsInDateRange.ToTimeZoneTime(timeZoneInfo_User);

            //To Fix DayLight saving time
            timeSlotsConvertedTimeZone = timeSlotsConvertedTimeZone.Where(e => e.Day <= toDate_UTC.Day).ToList();

            foreach (var dateGroup in timeSlotsConvertedTimeZone.GroupBy(e => e.Date))
            {
                var spotDate = dateGroup.Key;
                var slotsInDate = dateGroup.ToList();

                var timeSlotInDay = new TimeSpotsInDay { Date = spotDate };

                foreach (DateTime slot in slotsInDate)
                {
                    var slot_utc = slot.ToUniversalTime();
                    DateTimeOffset slotTime_user = slot_utc.GetDateTimeOffset(timeZoneInfo_User);

                    timeSlotInDay.Spots.Add(new TimeSpot
                    {
                        Remaining = 1,
                        Status = "Available",
                        StartTime = slot,
                        CalendarTime = slot_utc.ToTimeZoneTime(timeZoneInfo_Calendar),
                        slotTime_user = slotTime_user
                    });
                }

                timeSlotResponse.Days.Add(timeSlotInDay);

            }

            return timeSlotResponse;
        }

    }

}

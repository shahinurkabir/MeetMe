using MediatR;
using MeetMe.Application.Availabilities.Queries;
using MeetMe.Core.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("GetList")]
        public async Task<List<EventTimeAvailability>> GetList(string timezone,string from, string to)
        {
            //var from = "01-Jun-2023";
            //var to = "30-Jun-2023";
            var calendarTimeZone = "Bangladesh Standard Time";
            var bufferTime = 15;
            var meetingDuration = 30;

            var scheduleList = new List<AvailabilityDetails>() {
                                new AvailabilityDetails { ValueType="weekday", Value="Sunday", StartFrom= 540, EndAt= 1020 },
                                new AvailabilityDetails { ValueType="weekday", Value="Tuesday", StartFrom= 700, EndAt= 1020 },
                                new AvailabilityDetails { ValueType="weekday", Value="Wednesday", StartFrom= 540, EndAt= 1020 },
                               // new AvailabilityDetails { ValueType="date", Value="01-Jan-2023", StartFrom= 660, EndAt= 720 },
                            };
            var result = GetCalendarTimeSlots(timezone, calendarTimeZone, from, to, bufferTime, meetingDuration, scheduleList);

            return await Task.FromResult(result);
        }


        List<EventTimeAvailability> GetCalendarTimeSlots( string timezoneUser,string timezoneCalendar, string dateFrom, string dateTo, int bufferTime, int meetingDuration, List<AvailabilityDetails> scheduleList)
        {
            var result = new List<EventTimeAvailability>();
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneUser);
            var calendarTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneCalendar);
            var userTimeZoneOffset = userTimeZone.GetUtcOffset(DateTime.UtcNow);
            var serverTimeOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);

            var fromDateLocal = DateTimeOffset.Parse(dateFrom).Add(serverTimeOffset - userTimeZoneOffset);

            var dateUtc = fromDateLocal.ToUniversalTime();

            var fromDateUser = TimeZoneInfo.ConvertTime(fromDateLocal, userTimeZone);
            var fromDateCalendar = TimeZoneInfo.ConvertTime(fromDateUser, calendarTimeZone).Add(serverTimeOffset - userTimeZoneOffset);

            Console.WriteLine($"Date from local : {fromDateLocal}");
            Console.WriteLine($"Date from utc : {dateUtc}");
            Console.WriteLine($"Date from user : {fromDateUser}");
            Console.WriteLine($"Date from calendar : {fromDateCalendar}");

            var dayNumberInUserTimeZone = fromDateUser.Day;
            var daysInMonthUserTimeZone = DateTime.DaysInMonth(fromDateUser.Year, fromDateUser.Month);

            var scheduleDate = fromDateCalendar;

            while (dayNumberInUserTimeZone <= daysInMonthUserTimeZone)
            {
                Console.WriteLine($"Schedule day : {scheduleDate}");

                var weekDay = scheduleDate.DayOfWeek;

                var availability = scheduleList.FirstOrDefault(x => x.ValueType == "weekday" && x.Value == weekDay.ToString());
                var availabilityCustom = scheduleList.FirstOrDefault(x => x.ValueType == "date" && DateTime.Parse(x.Value) == scheduleDate.Date);

                if (availability != null || availabilityCustom != null)
                {
                    var dayStartFromInMinutes = 0d;
                    var dayEndFromINMinutes = 0d;

                    if (availabilityCustom != null)
                    {
                        dayStartFromInMinutes = availabilityCustom.StartFrom;
                        dayEndFromINMinutes = availabilityCustom.EndAt;
                    }
                    else if (availability != null)
                    {
                        dayStartFromInMinutes = availability.StartFrom;
                        dayEndFromINMinutes = availability.EndAt;
                    }
                    var timeSlots = GenerateTimeSlotForDate(scheduleDate, bufferTime, meetingDuration, dayStartFromInMinutes, dayEndFromINMinutes, userTimeZone);

                    var eventAvailability = new EventTimeAvailability
                    {
                        Date = scheduleDate.ToString("yyyy-MMM-dd"),
                        Slots = timeSlots.ToList()
                    };
                    result.Add(eventAvailability);
                }

                scheduleDate = scheduleDate.AddDays(1);

                dayNumberInUserTimeZone++;
            }
            return result;
        }



        IEnumerable<TimeSlot> GenerateTimeSlotForDate(
            DateTimeOffset calenderDate,
            double bufferTimeInMinute,
            int meetingDuration,
            double dayStartFromInMinutes,
            double dayEndInMinutes, TimeZoneInfo destnationTimeZone)
        {

            var dayStartTime = DateTimeOffset.Parse(calenderDate.ToString("yyyy-MM-dd ")).AddMinutes(dayStartFromInMinutes);
            var dayEndTime = DateTimeOffset.Parse(calenderDate.ToString("yyyy-MM-dd ")).AddMinutes(dayEndInMinutes);

            while (dayStartTime <= dayEndTime)
            {
                yield return new TimeSlot { StartAt = TimeZoneInfo.ConvertTime(dayStartTime, destnationTimeZone) };
                dayStartTime = dayStartTime.AddMinutes(meetingDuration).AddMinutes(bufferTimeInMinute); ;

            }

        }

        public class AvailabilityDetails
        {
            public string ValueType { get; set; } = null!;
            public string Value { get; set; } = null!;
            public double StartFrom { get; set; }
            public double EndAt { get; set; }
        }

        public class EventTimeAvailability
        {
            public EventTimeAvailability()
            {
                Slots=new List<TimeSlot>();
            }
            public string Date { get; set; } = null!;
            public List<TimeSlot> Slots { get; set; } = null!;
        }
        public class TimeSlot
        {
            public DateTimeOffset StartAt { get; set; }
        }
        public class CalendarDay
        {
            public string Date { get; set; } = null!;
            public List<DateTimeOffset> Spots { get; set; } = null!;
        }

    }
}

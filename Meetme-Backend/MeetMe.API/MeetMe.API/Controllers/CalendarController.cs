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
        [Route("")]
        public async Task<List<CalendarDay>> GetList()
        {
            var from = "01-jan-2023 03:00 AM";
            var to = "31-jan-2023";
            var bufferTime = 15;
            var meetingDuration = 30;

            var scheduleList = new List<AvailabilityDetails>() {
                                new AvailabilityDetails { ValueType="weekday", Value="Sunday", StartFrom= 540, EndAt= 1300 },
                                new AvailabilityDetails { ValueType="weekday", Value="Tuesday", StartFrom= 600, EndAt= 900 },
                                new AvailabilityDetails { ValueType="date", Value="01-Jan-2023", StartFrom= 660, EndAt= 720 },
                            };
            var result = GetCalendarTimeSlots(from, to, bufferTime, meetingDuration, scheduleList);

            return await Task.FromResult(result);
        }


        List<CalendarDay> GetCalendarTimeSlots(string dateFrom, string dateTo, int bufferTime, int meetingDuration, List<AvailabilityDetails> scheduleList)
        {
            var result = new List<CalendarDay>();
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Fiji Standard Time");
            var calendarTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
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

                    var calendarDay = new CalendarDay
                    {
                        Date = scheduleDate.ToString("yyyy-MM-dd"),
                        Spots = timeSlots.ToList()
                    };
                    result.Add(calendarDay);
                }

                scheduleDate = scheduleDate.AddDays(1);

                dayNumberInUserTimeZone++;
            }
            return result;
        }



        IEnumerable<DateTimeOffset> GenerateTimeSlotForDate(
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
                //yield return TimeZoneInfo.ConvertTime(dayStartTime, destnationTimeZone);
                yield return dayStartTime;
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

        public class CalendarDay
        {
            public string Date { get; set; } = null!;
            public List<DateTimeOffset> Spots { get; set; } = null!;
        }

    }
}

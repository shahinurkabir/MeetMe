using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.VisualBasic;

namespace MeetMe.Core.Extensions
{
    public static class DateTimeExtesions
    {

        public static (DateTime,DateTime) GetPastTimeByTimeZone(string timeZoneName)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            DateTime currentDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            DateTime pastDate = currentDate.AddDays(-1);
            DateTime startDateTime = DateTime.MinValue;
            DateTime endDateTime = new DateTime(pastDate.Year, pastDate.Month, pastDate.Day, 23, 59, 59);

            return (startDateTime.ToUniversalTime(), endDateTime.ToUniversalTime());
        }
        public static (DateTime, DateTime) GetUpcomingTimeByTimeZone(string timeZoneName)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            DateTime currentDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            DateTime upcomingDate = currentDate.AddDays(1);
            DateTime startDateTime = new DateTime(upcomingDate.Year, upcomingDate.Month, upcomingDate.Day, 0, 0, 0);
            DateTime endDateTime = DateTime.MaxValue;

            return (startDateTime.ToUniversalTime(), endDateTime.ToUniversalTime());
        }
        public static (DateTime, DateTime) GetTodayTimeByTimeZone(string timeZoneName)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            DateTime currentDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            DateTime startDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0);
            DateTime endDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 23, 59, 59);

            return (startDateTime.ToUniversalTime(), endDateTime.ToUniversalTime());
        }
        public static (DateTime, DateTime) GetTomorrowTimeByTimeZone(string timeZoneName)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            DateTime currentDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            DateTime tomorrowDate = currentDate.AddDays(1);
            DateTime startDateTime = new DateTime(tomorrowDate.Year, tomorrowDate.Month, tomorrowDate.Day, 0, 0, 0);
            DateTime endDateTime = new DateTime(tomorrowDate.Year, tomorrowDate.Month, tomorrowDate.Day, 23, 59, 59);

            return (startDateTime.ToUniversalTime(), endDateTime.ToUniversalTime());
        }
        public static (DateTime, DateTime) GetThisWeekTimeByTimeZone(string timeZoneName)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            DateTime currentDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            DateTime startDateTime = currentDate.Date.AddDays(-(int)currentDate.DayOfWeek);
            DateTime endDateTime = startDateTime.AddDays(-(int)currentDate.DayOfWeek + 7);

            return (startDateTime.ToUniversalTime(), endDateTime.ToUniversalTime());
        }
        public static (DateTime, DateTime) GetThisMonthTimeByTimeZone(string timeZoneName)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            DateTime currentDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            DateTime startDateTime = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime endDateTime = startDateTime.AddMonths(1).AddDays(-1);

            return (startDateTime.ToUniversalTime(), endDateTime.ToUniversalTime());
        }

        public static (DateTime,DateTime) GetDateRangeByTimeZone(string timeZoneName,DateTime startDate,DateTime endDate)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
            DateTime startDateTime = TimeZoneInfo.ConvertTime(startDate, timeZoneInfo);
            DateTime endDateTime = TimeZoneInfo.ConvertTime(endDate, timeZoneInfo);
            
            if (startDateTime==endDateTime)
            {
                endDateTime = endDateTime.AddDays(1);
            }

            return (startDateTime.ToUniversalTime(),endDateTime.ToUniversalTime());
        }
        public static string ToTimeZoneFormattedText(this DateTime dateTime, string format, string timeZomeId)
        {

            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZomeId);
            var dateTimeConverted = dateTime.ToTimeZoneTime(timeZoneInfo);

            return dateTimeConverted.ToString(format);
        }

        public static DateTime ToTimeZoneTime(this DateTime time, TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTime(time, timeZoneInfo);
        }


        public static DateTimeOffset GetDateTimeOffset(this DateTime time, TimeZoneInfo timeZoneInfo)
        {
            // here, is where you need to convert
            if (time.Kind != DateTimeKind.Unspecified)
                time = TimeZoneInfo.ConvertTime(time, timeZoneInfo);

            TimeSpan offset = timeZoneInfo.GetUtcOffset(time);

            return new DateTimeOffset(time, offset);
        }

        public static DateTime ToUtcIfLocal(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Local)
            {
                return dateTime.ToUniversalTime();
            }
            return dateTime;
        }

    }
}

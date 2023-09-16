using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MeetMe.Core.Extensions
{
    public static class DateTimeExtesions
    {
        public static DateTime ToTimeZoneTime(this DateTime time, string timeZomeId)
        {

            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZomeId);
            return time.ToTimeZoneTime(timeZoneInfo);
        }

        public static DateTime ToTimeZoneTime(this DateTime time, TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTime(time, timeZoneInfo);
        }

        public static List<DateTime> ToTimeZoneTime(this List<DateTime> times, TimeZoneInfo timeZoneInfo)
        {
            return times.Select(e => e.ToTimeZoneTime(timeZoneInfo)).ToList();
        }
        public static DateTimeOffset GetDateTimeOffset(this DateTime time, string timeZomeId)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZomeId);

            return time.GetDateTimeOffset(timeZoneInfo);
        }

        public static DateTimeOffset GetDateTimeOffset(this DateTime time, TimeZoneInfo timeZoneInfo)
        {
            // here, is where you need to convert
            if (time.Kind != DateTimeKind.Unspecified)
                time = TimeZoneInfo.ConvertTime(time, timeZoneInfo);

            TimeSpan offset = timeZoneInfo.GetUtcOffset(time);

            return new DateTimeOffset(time, offset);
        }
        public static string ToAppointmentTimeRangeText(this string timeZoneName, int meetingDuration, DateTime appointmentStartTime)
        {
            var dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(appointmentStartTime, TimeZoneInfo.Utc.Id, timeZoneName);
            var startTime = dateTime.ToString("hh:mm tt");
            var endTime = dateTime.AddMinutes(meetingDuration).ToString("hh:mm tt");
            var appointmentTimeRange = $"{startTime} - {endTime}, {dateTime.ToString("dddd, MMMM dd, yyyy")}";

            return appointmentTimeRange;
        }
    }
}

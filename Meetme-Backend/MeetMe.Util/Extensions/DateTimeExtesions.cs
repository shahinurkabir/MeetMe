using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MeetMe.Utility.Extensions
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
    }
}

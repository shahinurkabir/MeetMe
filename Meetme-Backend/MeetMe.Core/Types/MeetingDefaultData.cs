using System;
using System.Collections.Generic;

namespace MeetMe.Core.Types
{
    public static class MeetingDefaultData
    {
        public const int MEETING_DURATION = 30; //minutes
        public const int MEETING_FORWARD_DURATION = 24 * 60 * 60; // 60 days in minutes
        public const string MEETING_FROM_TIMESPAN = "9:00:00";
        public const string MEETING_TO_TIMESPAN = "17:00:00";
        public const string SCHEDULE_DATETYPE_WEEKDAY = "weekday";
        public const string SCHEDULE_DATETYPE_DATE = "date";

        public static readonly Dictionary<int, string> WeekDays = new Dictionary<int, string>
        {
            {0,"Sunday" },
            {1,"Monday" },
            {2,"Tuesday" },
            {3,"Wednesday" },
            {4,"Thursday" },
            {5,"Friday" },
            {6,"Saturday" }
        };
    }


}

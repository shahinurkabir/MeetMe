using System;
using System.Collections.Generic;

namespace MeetMe.Core.Constant
{
    public static class Constants
    {
        //public const int MEETING_DURATION_MINUTES = 30; //minutes
        //public const int MEETING_FORWARD_DURATION = 24 * 60 * 60; // 60 days in minutes
        //public const string MEETING_FROM_TIMESPAN = "9:00:00";
        //public const string MEETING_TO_TIMESPAN = "17:00:00";
        //public const string SCHEDULE_DATETYPE_WEEKDAY = "weekday";
        //public const string SCHEDULE_DATETYPE_DATE = "date";

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

        public static class Events
        {
            public const int BufferTimeDuration = 30;// minutes
            public const int MeetingDuration = 30; // minutes
            public const int ForwardDuration = 24 * 60 * 60; // 60 days
            public const string SCHEDULE_DATETYPE_WEEKDAY = "weekday";
            public const string SCHEDULE_DATETYPE_DATE = "date";

            public const string MEETING_FROM_TIMESPAN = "9:00:00";
            public const string MEETING_TO_TIMESPAN = "17:00:00";

            public static class ForwandDateKInd
            {
                public const string Moving = "moving";
                public const string DateRange = "daterange";
                public const string Foreever = "foreever";
            }


        }


    }
}

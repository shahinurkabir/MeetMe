using System;

namespace MeetMe.Application.EventTypes.Queries.TimeSpots
{
    public class TimeSpot
    {
        public DateTimeOffset slotTime_user { get; set; }
        public DateTime CalendarTime { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public string Status { get; set; }
        public int Remaining { get; set; }
    }

}

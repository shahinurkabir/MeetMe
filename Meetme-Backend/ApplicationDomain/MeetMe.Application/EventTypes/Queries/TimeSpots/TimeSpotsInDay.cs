using System;
using System.Collections.Generic;

namespace MeetMe.Application.EventTypes.Queries.TimeSpots
{
    public class TimeSpotsInDay
    {
        public TimeSpotsInDay()
        {
            Spots = new List<TimeSpot>();
        }
        public DateTime Date { get; set; }
        public List<TimeSpot> Spots { get; set; }
    }

}

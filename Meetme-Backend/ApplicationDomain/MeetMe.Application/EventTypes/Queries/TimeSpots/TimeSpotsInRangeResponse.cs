using System.Collections.Generic;

namespace MeetMe.Application.EventTypes.Queries.TimeSpots
{
    public class TimeSpotsInRangeResponse
    {
        public TimeSpotsInRangeResponse()
        {
            Days = new List<TimeSpotsInDay>();
        }
        public List<TimeSpotsInDay> Days { get; set; }

    }

}

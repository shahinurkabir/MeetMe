using MediatR;
using System;
using System.Text;
using MeetMe.Core;

namespace MeetMe.Application.EventTypes.Queries.TimeSpots
{

    public class GetTimeSpotsInRangeQuery : IRequest<TimeSpotsInRangeResponse>
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string TimeZoneId { get; set; }
        public Guid CalendarId { get; set; }
        public string Month { get; set; }

    }

}

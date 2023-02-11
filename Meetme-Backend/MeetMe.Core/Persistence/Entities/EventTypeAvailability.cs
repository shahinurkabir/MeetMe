using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MeetMe.Core.Persistence.Entities
{
    public class EventTypeAvailability
    {

        public EventTypeAvailability()
        {
            AvailabilityDetails = new List<EventTypeAvailabilityDetail>();
        }
        public Guid Id { get; set; }
        public string DateForwardKind { get; set; } = null!;
        public int? ForwardDuration { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Duration { get; set; }
        public int BufferTimeBefore { get; set; }
        public int BufferTimeAfter { get; set; }
        public Guid? BaseScheduleId { get; set; }
        public int TimeZoneId { get; set; }

        [JsonIgnore]
        public EventType EventType { get; set; }

        public List<EventTypeAvailabilityDetail> AvailabilityDetails { get; set; } 

    }

    
}


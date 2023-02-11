using System;
using System.Text.Json.Serialization;

namespace MeetMe.Core.Persistence.Entities
{
    public class EventTypeAvailabilityDetail
    {
        public Guid Id { get; set; }
        public Guid AvailabilityId { get; set; }

        /// <summary>
        /// D:Date
        /// W:Weekday
        /// </summary>
        public string Type { get; set; }
        public string Day { get; set; }
        public DateTime? Date { get; set; }
        public short StepId { get; set; }
        public double From { get; set; }
        public double To { get; set; }

        [JsonIgnore]
        public EventTypeAvailability Availability { get; set; } 
    }
}

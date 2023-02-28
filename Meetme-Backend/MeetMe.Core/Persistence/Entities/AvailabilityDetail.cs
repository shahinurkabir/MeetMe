using System;
using System.Text.Json.Serialization;

namespace MeetMe.Core.Persistence.Entities
{
    public class AvailabilityDetail
    {
        public int Id { get; set; }
        public Guid RuleId { get; set; }

        [JsonIgnore]
        public Availability? Availability { get; set; }
        /// <summary>
        /// D:Date
        /// W:Weekday
        /// </summary>
        public string DayType { get; set; } = null!;
        public string? Value { get; set; }
        public short StepId { get; set; }
        public double From { get; set; }
        public double To { get; set; }
    }
}

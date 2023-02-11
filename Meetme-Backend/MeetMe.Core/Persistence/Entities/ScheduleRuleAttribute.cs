using System;
using System.Text.Json.Serialization;

namespace MeetMe.Core.Persistence.Entities
{
    public class ScheduleRuleAttribute
    {
        public int Id { get; set; }
        public Guid RuleId { get; set; }

        [JsonIgnore]
        public ScheduleRule ScheduleRule { get; set; }
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
    }
}

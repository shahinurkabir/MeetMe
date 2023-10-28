using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MeetMe.Core.Persistence.Entities
{
    public class EventTypeAvailabilityDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid EventTypeId { get; set; }

        /// <summary>
        /// D:Date
        /// W:Weekday
        /// </summary>
        public string DayType { get; set; } = null!;
        public string Value { get; set; } = null!;
        public short StepId { get; set; }
        public double From { get; set; }
        public double To { get; set; }

        [JsonIgnore]
        public EventType? EventType { get; set; }=null!;
    }
}

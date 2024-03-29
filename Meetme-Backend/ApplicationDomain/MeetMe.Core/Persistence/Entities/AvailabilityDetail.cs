﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MeetMe.Core.Persistence.Entities
{
    public class AvailabilityDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid AvailabilityId { get; set; }

        [JsonIgnore]
        public Availability? Availability { get; set; }
        /// <summary>
        /// D:Date
        /// W:Weekday
        /// </summary>
        public string DayType { get; set; } = null!;
        public string Value { get; set; } = null!;
        public short StepId { get; set; }
        public double From { get; set; }
        public double To { get; set; }
    }
}

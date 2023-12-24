using MeetMe.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MeetMe.Core.Persistence.Entities
{
    public class EventType : ISoftDelete
    {
        public EventType()
        {
            Questions = new List<EventTypeQuestion>();
            EventTypeAvailabilityDetails = new List<EventTypeAvailabilityDetail>();
        }
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string Slug { get; set; } = null!;
        public string EventColor { get; set; } = null!;
        public bool ActiveYN { get; set; }
        public bool IsDeleted { get; set; }
        public string TimeZone { get; set; }=null!;
        public string DateForwardKind { get; set; } = null!;
        public int ForwardDurationInDays { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Duration { get; set; }
        public int BufferTimeBefore { get; set; }
        public int BufferTimeAfter { get; set; }
        public Guid? AvailabilityId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        //[JsonIgnore]
        public List<EventTypeQuestion> Questions { get; set; } = null!;

        [JsonIgnore]
        public List<EventTypeAvailabilityDetail> EventTypeAvailabilityDetails { get; set; } = null!;

        [JsonIgnore]
        public List<Appointment> Appointments { get; set; }=null!;

        [JsonIgnore]
        public User User { get; set; }=null!;

    }

}


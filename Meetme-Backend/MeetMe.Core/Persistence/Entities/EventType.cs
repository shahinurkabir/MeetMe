using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Persistence.Entities
{
    public class EventType
    {

        public EventType()
        {
            Questions = new List<EventTypeQuestion>();
        }
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } 
        public string? Location { get; set; }
        public string Slug { get; set; } = null!;
        public string EventColor { get; set; }=null!;
        public bool ActiveYN { get; set; }
        public int TimeZoneId { get; set; }
        public string DateForwardKind { get; set; } = null!;
        public int? ForwardDuration { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Duration { get; set; }
        public int BufferTimeBefore { get; set; }
        public int BufferTimeAfter { get; set; }
        public string? CustomAvailability { get; set; }
        public Guid? AvailabilityId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        public List<EventTypeQuestion> Questions { get; set; }

        public EventTypeAvailability EventTypeAvailability { get; set; }

    }

    public class EventTypeScheduleInfo
    {
        public EventTypeScheduleInfo()
        {
            WeeklyTimeSchedule = new List<DailyTimeSchedule>();
        }

        public Guid EventTypeId { get; set; }
        public List<DailyTimeSchedule> WeeklyTimeSchedule { get; set; }

    }
}


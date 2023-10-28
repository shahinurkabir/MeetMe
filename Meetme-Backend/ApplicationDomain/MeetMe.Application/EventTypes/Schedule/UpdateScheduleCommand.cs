using MediatR;
using System;
using System.Collections.Generic;

namespace MeetMe.Application.EventTypes.Schedule
{
    public class UpdateScheduleCommand : IRequest<bool>
    {
        public UpdateScheduleCommand()
        {
            WeeklyTimeSlots = new List<DailyTimeSlot>();
        }

        public Guid CalendarId { get; set; }
        public string DateForwardKind { get; set; }
        public int ForwardDuration { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Duration { get; set; }
        public int BufferTimeBefore { get; set; }
        public int BufferTimeAfter { get; set; }
        public string TimeZoneId { get; set; }
        public List<DailyTimeSlot> WeeklyTimeSlots { get; set; }

    }

    public class DailyTimeSlot
    {
        public string Type { get; set; }
        public string Day { get; set; }
        public DateTime? Date { get; set; }
        public double From { get; set; }
        public double To { get; set; }
    }
}

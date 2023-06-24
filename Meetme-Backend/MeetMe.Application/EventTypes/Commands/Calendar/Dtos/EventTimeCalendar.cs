using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.EventTypes.Calendar.Dtos
{
    public class EventTimeCalendar
    {
        public EventTimeCalendar()
        {
            Slots = new List<TimeSlot>();
        }
        public string Date { get; set; } = null!;
        public List<TimeSlot> Slots { get; set; } = null!;
    }
    public class TimeSlot
    {
        public DateTimeOffset StartAt { get; set; }
    }
}

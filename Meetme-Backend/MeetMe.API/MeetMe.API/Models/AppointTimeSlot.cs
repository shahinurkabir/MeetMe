using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetMe.API.Models
{
    public class CalendarDate
    {
        public CalendarDate()
        {
            Slots = new List<TimeSlot>();
        }
        public string Date { get; set; }
        public List<TimeSlot> Slots { get; set; }

    }
    public class TimeSlot
    {
        public string StartTime { get; set; }
        public string Status { get; set; }
        public int InviteeRemaining { get; set; }

    }
}

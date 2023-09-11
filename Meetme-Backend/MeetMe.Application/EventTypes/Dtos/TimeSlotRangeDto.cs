using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.EventTypes.Dtos
{
    public class TimeSlotRangeDto
    {
        public TimeSlotRangeDto()
        {
            Slots = new List<TimeSlot>();
        }
        public string Date { get; set; } = null!;
        public List<TimeSlot> Slots { get; set; } = null!;
    }
    public class TimeSlot
    {
        public DateTimeOffset StartDateTime { get; set; }
    }
}

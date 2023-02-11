using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Application.ScheduleRules.Dtos
{
    public class AppointmentScheduleItemDto
    {
        public int Id { get; set; }
        public Guid AppointmentId { get; set; }
        public string Type { get; set; }
        public string Day { get; set; }
        public DateTime? Date { get; set; }
        public double From { get; set; }
        public double To { get; set; }
    }
}

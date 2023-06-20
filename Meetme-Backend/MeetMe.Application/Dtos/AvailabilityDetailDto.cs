using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.Dtos
{
    public class AvailabilityDetailDto
    {
        public string DayType { get; set; } = null!;
        public string Value { get; set; } = null!;
        public double StartFrom { get; set; }
        public double EndAt { get; set; }

    }
}

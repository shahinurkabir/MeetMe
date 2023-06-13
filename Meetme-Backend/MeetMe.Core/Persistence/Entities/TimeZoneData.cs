using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Persistence.Entities
{
    public class TimeZoneData
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Offset { get; set; }=null!;
        public int OffsetMinutes { get; set; }
    }
}

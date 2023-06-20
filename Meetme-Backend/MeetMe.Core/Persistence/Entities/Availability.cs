using MeetMe.Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Persistence.Entities
{
    public class Availability : ISoftDelete
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid OwnerId { get; set; }
        public string TimeZone { get; set; }
        public bool IsCustom { get; set; }
        public bool IsDefault { get; set; }
        public List<AvailabilityDetail> Details { get; set; }
        public bool IsDeleted { get; set; }

        public Availability()
        {
            Details = new List<AvailabilityDetail>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Core.Persistence.Entities
{
    public class ScheduleRule
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OwnerId { get; set; }
        public int TimeZoneId { get; set; }
        public List<ScheduleRuleAttribute> RuleAttributes { get; set; }
        public ScheduleRule()
        {
            RuleAttributes = new List<ScheduleRuleAttribute>();
        }
    }
}

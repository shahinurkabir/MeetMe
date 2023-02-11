using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Application.ScheduleRules.Dtos;

namespace MeetMe.Application.ScheduleRules.Commands
{
    public class CreateScheduleRuleCommand : IRequest<Guid>
    {
        public CreateScheduleRuleCommand()
        {
            RuleItems = new List<ScheduleRuleAttribute>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TimeZoneId { get; set; }
        public List<ScheduleRuleAttribute> RuleItems { get; set; }
    }

}

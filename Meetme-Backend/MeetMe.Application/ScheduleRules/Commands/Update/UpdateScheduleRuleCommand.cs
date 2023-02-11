﻿using MediatR;
using System;
using System.Collections.Generic;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.ScheduleRules.Commands.Update
{
    public class UpdateScheduleRuleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TimeZoneId { get; set; }
        public List<ScheduleRuleAttribute> RuleItems { get; set; }
    }

}

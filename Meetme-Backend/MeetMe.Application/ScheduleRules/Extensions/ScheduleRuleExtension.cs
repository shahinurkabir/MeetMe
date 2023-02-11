using MeetMe.Core.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Application.ScheduleRules.Extensions
{
    public static class ScheduleRuleExtension
    {
        public static List<ScheduleRuleAttribute> ToEntityList(this List<ScheduleRuleAttribute> scheduleRuleAttributes, Guid scheduleRuleId)
        {
            var listScheduleRuleItems = new List<ScheduleRuleAttribute>();

            int itemId = 0;

            scheduleRuleAttributes.ForEach(e =>
            {
                var item = new ScheduleRuleAttribute
                {
                    Id = itemId,
                    RuleId = scheduleRuleId,
                    Type = e.Type,
                    Day = e.Day,
                    Date = e.Date,
                    StepId=e.StepId,
                    From = e.From,
                    To = e.To
                };

                listScheduleRuleItems.Add(item);

                itemId++;

            });

            return listScheduleRuleItems;

        }
    }
}

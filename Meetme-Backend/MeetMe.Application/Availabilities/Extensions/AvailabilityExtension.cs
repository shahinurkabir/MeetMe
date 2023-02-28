using MeetMe.Core.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetMe.Application.Availabilities.Extensions
{
    public static class AvailabilityExtension
    {
        public static List<AvailabilityDetail> ToEntityList(this List<AvailabilityDetail> scheduleRuleAttributes, Guid scheduleRuleId)
        {
            var listScheduleRuleItems = new List<AvailabilityDetail>();

            int itemId = 0;

            scheduleRuleAttributes.ForEach(e =>
            {
                var item = new AvailabilityDetail
                {
                    Id = itemId,
                    RuleId = scheduleRuleId,
                    DayType = e.DayType,
                    Value = e.Value,
                    StepId = e.StepId,
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

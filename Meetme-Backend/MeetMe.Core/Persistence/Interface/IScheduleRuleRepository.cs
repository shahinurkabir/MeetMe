using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Persistence.Interface
{
    public interface IScheduleRuleRepository
    {
        Task<bool> AddSchedule(ScheduleRule scheduleRule);
        Task<bool> UpdateSchedule(ScheduleRule scheduleRule);
        Task<bool> DeleteSchedule(Guid id);

        Task<ScheduleRule> GetScheduleById(Guid id);
        Task<List<ScheduleRule>> GetScheduleListByUserId(Guid userId);

       // Task<AppointmentSchedule> GetScheduleListByRuleId(Guid ruleId);
    }
}

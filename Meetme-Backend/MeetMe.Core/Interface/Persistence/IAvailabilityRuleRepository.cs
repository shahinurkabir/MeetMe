using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Core.Interface.Persistence
{
    public interface IAvailabilityRuleRepository
    {
        Task<bool> AddAvailabilityRule(AvailabilityRule availabilityRule);
        Task<bool> DeleteAvailabilityRule(Guid ruleId);

        Task<AvailabilityRule> GetAvailabilityRuleById(Guid ruleId);
        Task<List<AvailabilityRule>> GetSchedulesByUserId(Guid userId);
    }
}

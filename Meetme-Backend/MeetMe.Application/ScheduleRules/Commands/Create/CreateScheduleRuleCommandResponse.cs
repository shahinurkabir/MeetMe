using System;

namespace MeetMe.Application.ScheduleRules.Commands
{
    public class CreateScheduleRuleCommandResponse
    {
        public Guid Id { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
    }
}

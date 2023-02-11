using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Application.ScheduleRules.Commands.Update;

namespace MeetMe.Application.ScheduleRules.Commands
{
    public class UpdateScheduleRuleCommandValidator : AbstractValidator<UpdateScheduleRuleCommand>
    {
        public UpdateScheduleRuleCommandValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name can not be empty");
            RuleFor(m => m.TimeZoneId).NotEmpty().WithMessage("TimeZone is empty.");
            RuleFor(m => m.RuleItems).Must(RequiredScheduleItems).WithMessage("No Schedule items found.");
        }
        private bool RequiredScheduleItems(IEnumerable<ScheduleRuleAttribute> ruleAttributes)
        {
            return ruleAttributes.Any();
        }

    }
}

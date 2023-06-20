using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.Availabilities.Commands.Update
{
    public class UpdateAvailabilityCommandValidator : AbstractValidator<UpdateAvailabilityCommand>
    {
        public UpdateAvailabilityCommandValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name can not be empty");
            RuleFor(m => m.TimeZone).NotEmpty().WithMessage("TimeZone is empty.");
            RuleFor(m => m.Details).Must(RequiredScheduleItems).WithMessage("No Schedule items found.");
        }
        private bool RequiredScheduleItems(IEnumerable<AvailabilityDetail> ruleAttributes)
        {
            return ruleAttributes.Any();
        }

    }
}

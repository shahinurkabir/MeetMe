using FluentValidation;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.Availabilities.Commands.Create
{
    public class CreateAvailabilityCommandValidator : AbstractValidator<CreateAvailabilityCommand>
    {
        public CreateAvailabilityCommandValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name can not be empty");
            RuleFor(m => m.TimeZoneId).NotEmpty().WithMessage("TimeZone is empty.");
            RuleFor(m => m.Details).Must(RequiredScheduleItems).WithMessage("No Schedule items found.");
        }
        private bool RequiredScheduleItems(IEnumerable<AvailabilityDetail> ruleAttributes)
        {
            return ruleAttributes.Any();
        }

    }
}

using FluentValidation;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.Availabilities.Commands.Create
{
    public class EditNameAvailabilityCommandValidator : AbstractValidator<CreateAvailabilityCommand>
    {
        public EditNameAvailabilityCommandValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name can not be empty");
        }

    }
}

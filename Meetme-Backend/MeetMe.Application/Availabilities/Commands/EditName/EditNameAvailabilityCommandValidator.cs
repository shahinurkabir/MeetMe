using FluentValidation;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.Availabilities.Commands.EditName
{
    public class EditNameAvailabilityCommandValidator : AbstractValidator<EditAvailabilityNameCommand>
    {
        public EditNameAvailabilityCommandValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name can not be empty");
        }

    }
}

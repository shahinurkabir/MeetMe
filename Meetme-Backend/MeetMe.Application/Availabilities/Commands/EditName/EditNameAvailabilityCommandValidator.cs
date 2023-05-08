using FluentValidation;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.Availabilities.Commands.EditName
{
    public class SetDefaultCommandValidator : AbstractValidator<SetDefaultCommand>
    {
        public SetDefaultCommandValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name can not be empty");
        }

    }
}

using FluentValidation;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.Availabilities.Commands.SetDefault
{
    public class SetDefaultCommandValidator : AbstractValidator<SetDefaultAvailabilityCommand>
    {
        public SetDefaultCommandValidator()
        {
        }

    }
}

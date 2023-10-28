using FluentValidation;
using System;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;

namespace MeetMe.Application.Bookings.Commands.New
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        private readonly IPersistenceProvider persistenseProvider;

        public CreateBookingCommandValidator(IPersistenceProvider persistenseProvider)
        {
            this.persistenseProvider = persistenseProvider;

            RuleFor(m => m.FullName).NotEmpty().WithMessage("Full Name cannot be empty");
            RuleFor(m => m.Email).NotEmpty().WithMessage("Email can not be empty.");
            RuleFor(m => m.EventTypeId).NotEqual(Guid.Empty).WithMessage("Invalid event type selected.");
            RuleFor(m => m.StartTime).NotEqual(new DateTime()).WithMessage("Invalid time spot provided");
            RuleFor(m => m.StartTime).LessThanOrEqualTo(DateTime.Now).WithMessage("This time spot already passed.");
        }
    }
}

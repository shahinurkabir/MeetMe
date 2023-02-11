using FluentValidation;
using MeetMe.Core.Types;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Schedule
{
    public class UpdateScheduleValidator : AbstractValidator<UpdateScheduleCommand>
    {
        private readonly IPersistenceProvider persistenseProvider;

        public UpdateScheduleValidator(IPersistenceProvider persistenseProvider)
        {
            this.persistenseProvider = persistenseProvider;

            RuleFor(m => m.TimeZoneId).NotEmpty().WithMessage("Calnedar time-zone cannot be empty.");

            RuleFor(m => m.DateForwardKind).NotEmpty().WithMessage("Date Range cannot be empty.");

            When(m => m.DateForwardKind == DateForwardKind.Moving.ToString().ToLower(), () =>
            {
                RuleFor(m => m.ForwardDuration).NotEmpty().WithMessage("Date Forward number cannot be empty.")
                  .GreaterThan(0).WithMessage("Date Forward must be greater then zero.");
            });

            When(m => m.DateForwardKind == DateForwardKind.DateRange.ToString().ToLower(), () =>
            {
                RuleFor(m => m.DateFrom).NotEmpty().WithMessage("Date start cannot be empty.");
                RuleFor(m => m.DateTo).NotEmpty().WithMessage("Date start cannot be empty.")
                      .GreaterThan(s => s.DateFrom).WithMessage("Date to cannot be less than start date.");
            });

            RuleFor(m => m.Duration).NotEmpty().WithMessage("Meeting duration cannot be empty.")
                .GreaterThan(0).WithMessage("Meeting duration must be greater then zero.");

        }

    }
}

using MediatR;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;
using FluentValidation;

namespace MeetMe.Application.Availabilities.Commands
{
    public class UpdateAvailabilityCommand : IRequest<bool>
    {
        public UpdateAvailabilityCommand()
        {
            Details = new List<AvailabilityDetail>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string TimeZone { get; set; } = null!;
        public List<AvailabilityDetail> Details { get; set; }
    }

    public class UpdateAvailabilityCommandHandler : IRequestHandler<UpdateAvailabilityCommand, bool>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public UpdateAvailabilityCommandHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }
        public async Task<bool> Handle(UpdateAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var entity = await persistenceProvider.GetAvailability(request.Id);

            if (entity == null)
            {
                throw new Core.Exceptions.MeetMeException("Invalid schedule provided.");
            }

            var listScheduleRuleItems = request.Details.Select(e => new AvailabilityDetail
            {
                AvailabilityId = entity.Id,
                DayType = e.DayType,
                Value = e.Value,
                From = e.From,
                To = e.To
            }).ToList();

            entity.Name = request.Name;
            entity.TimeZone = request.TimeZone;

            entity.Details = listScheduleRuleItems;

            await persistenceProvider.UpdateAvailability(entity);

            return true;

        }

    }

    public class UpdateAvailabilityCommandValidator : AbstractValidator<UpdateAvailabilityCommand>
    {
        public UpdateAvailabilityCommandValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name can not be empty");
            RuleFor(m => m.TimeZone).NotEmpty().WithMessage("TimeZone is empty.");
            RuleFor(m => m.Details).Must(RequiredScheduleItems).WithMessage("No Schedule items found.");
        }
        private bool RequiredScheduleItems(List<AvailabilityDetail> ruleAttributes)
        {
            return ruleAttributes.Any();
        }

    }
}



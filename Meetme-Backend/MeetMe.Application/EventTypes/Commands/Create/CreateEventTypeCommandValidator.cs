using FluentValidation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;

namespace MeetMe.Application.EventTypes.Commands.Create
{
    public class CreateCreateEventTypeCommandValidator : AbstractValidator<CreateEventTypeCommand>
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly ILoginUserInfo applicationUser;

        public CreateCreateEventTypeCommandValidator(IEventTypeRepository eventTypeRepository, ILoginUserInfo applicationUser)
        {
            this.eventTypeRepository = eventTypeRepository;
            this.applicationUser = applicationUser;

            RuleFor(m => m.Name).NotEmpty().WithMessage("Event Type name cannot be empty.");

            RuleFor(m => m.EventColor).NotEmpty().WithMessage("Event Color cannot be empty.");

            RuleFor(m => m.Slug).NotEmpty().WithMessage("Slug cannot be empty.")
                .MustAsync(async (model, slug, token) =>
                {
                    return await CheckNotUsed(model, token);

                }).WithMessage("Slug already used.");

        }

        private async Task<bool> CheckNotUsed(CreateEventTypeCommand command, CancellationToken cancellationToken)
        {
            var listEvents = await eventTypeRepository.GetEventTypeListByUserId(applicationUser.Id);

            var isUsed = listEvents.Count(e =>
            e.Slug.Equals(command.Slug, StringComparison.InvariantCultureIgnoreCase)) > 0;

            return isUsed == false;
        }
    }
}

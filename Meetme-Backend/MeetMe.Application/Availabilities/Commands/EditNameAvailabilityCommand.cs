using FluentValidation;
using MediatR;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Availabilities.Commands
{
    public class EditNameAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class EditNameAvailabilityCommandHandler : IRequestHandler<EditNameAvailabilityCommand, bool>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public EditNameAvailabilityCommandHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }
        public async Task<bool> Handle(EditNameAvailabilityCommand request, CancellationToken cancellationToken)
        {

            await persistenceProvider.UpdateAvailabilityName(request.Id, request.Name);

            return true;

        }
    }
    public class EditNameAvailabilityCommandValidator : AbstractValidator<EditNameAvailabilityCommand>
    {
        public EditNameAvailabilityCommandValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name can not be empty");
        }

    }

}

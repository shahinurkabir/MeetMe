using FluentValidation;
using MediatR;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Availabilities.Commands.EditName
{
    public class EditNameAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class EditNameAvailabilityCommandHandler : IRequestHandler<EditNameAvailabilityCommand, bool>
    {
        private readonly IAvailabilityRepository availabilityRepository;

        public EditNameAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository)
        {
            this.availabilityRepository = availabilityRepository;
        }
        public async Task<bool> Handle(EditNameAvailabilityCommand request, CancellationToken cancellationToken)
        {
            
            await availabilityRepository.EditName(request.Id, request.Name);

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

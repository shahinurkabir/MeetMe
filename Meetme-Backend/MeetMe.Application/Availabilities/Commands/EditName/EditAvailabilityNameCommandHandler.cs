using MediatR;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Availabilities.Commands.EditName
{
    public class SetDefaultCommandHandler : IRequestHandler<SetDefaultCommand, bool>
    {
        private readonly IAvailabilityRepository availabilityRepository;

        public SetDefaultCommandHandler(IAvailabilityRepository availabilityRepository)
        {
            this.availabilityRepository = availabilityRepository;
        }
        public async Task<bool> Handle(SetDefaultCommand request, CancellationToken cancellationToken)
        {
            
            await availabilityRepository.EditName(request.Id, request.Name);

            return true;
            
        }
    }

}

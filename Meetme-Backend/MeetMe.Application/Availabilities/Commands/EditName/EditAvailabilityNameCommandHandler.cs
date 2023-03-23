using MediatR;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Availabilities.Commands.EditName
{
    public class EditAvailabilityNameCommandHandler : IRequestHandler<EditAvailabilityNameCommand, bool>
    {
        private readonly IAvailabilityRepository availabilityRepository;

        public EditAvailabilityNameCommandHandler(IAvailabilityRepository availabilityRepository)
        {
            this.availabilityRepository = availabilityRepository;
        }
        public async Task<bool> Handle(EditAvailabilityNameCommand request, CancellationToken cancellationToken)
        {
            
            await availabilityRepository.EditName(request.Id, request.Name);

            return true;
            
        }
    }

}

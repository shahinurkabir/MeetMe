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
            var availabilityEntity = await availabilityRepository.GetScheduleById(request.Id);

            availabilityEntity.Name = request.Name;
            
            await availabilityRepository.UpdateSchedule(availabilityEntity);

            return true;
            
        }
    }

}

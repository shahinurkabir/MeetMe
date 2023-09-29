using MediatR;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Availabilities.Commands
{
    public class DeleteAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteAvailabilityCommandHandler : IRequestHandler<DeleteAvailabilityCommand, bool>
    {
        private readonly IAvailabilityRepository _availabilityRepository;

        public DeleteAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
        }

        public async Task<bool> Handle(DeleteAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _availabilityRepository.GetAvailability(request.Id);

            if (entity == null)
            {
                throw new Exception("Invalid id provided");
            }

            var result = await _availabilityRepository.DeleteAvailability(entity);

            return result;
        }
    }
}

using MediatR;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Availabilities.Commands.Delete
{
    public class DeleteAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteAvailabilityCommandHandler : IRequestHandler<DeleteAvailabilityCommand, bool>
    {
        private readonly IAvailabilityRepository availabilityRepository;

        public DeleteAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository)
        {
            this.availabilityRepository = availabilityRepository;
        }

        public async Task<bool> Handle(DeleteAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var entity = await availabilityRepository.GetById(request.Id);

            if (entity == null) throw new Exception("Invalid rule id provided");

            var result = await availabilityRepository.DeleteSchedule(entity);

            return result;
        }
    }
}

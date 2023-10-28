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
        private readonly IPersistenceProvider persistenceProvider;

        public DeleteAvailabilityCommandHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }

        public async Task<bool> Handle(DeleteAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var entity = await persistenceProvider.GetAvailability(request.Id);

            if (entity == null)
            {
                throw new Exception("Invalid id provided");
            }

            var result = await persistenceProvider.DeleteAvailability(entity);

            return result;
        }
    }
}

using MediatR;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Manage
{
    public class DeactivateEventTypeCommand : IRequest<bool>
    {
        public readonly Guid eventTypeId;

        public DeactivateEventTypeCommand(Guid eventTypeId)
        {
            this.eventTypeId = eventTypeId;
        }
    }
    public class DeactivateEventTypeCommandHandler : IRequestHandler<DeactivateEventTypeCommand, bool>
    {
        private readonly IEventTypeRepository eventTypeRepository;

        public DeactivateEventTypeCommandHandler(IEventTypeRepository eventTypeRepository)
        {
            this.eventTypeRepository = eventTypeRepository;
        }
        public async Task<bool> Handle(DeactivateEventTypeCommand request, CancellationToken cancellationToken)
        {
            var eventType = await eventTypeRepository.GetEventTypeById(request.eventTypeId);

            if (eventType == null)
                throw new Exception("Not found.");

            if (!eventType.ActiveYN)
                throw new Exception("Already Deactivated.");

            eventType.ActiveYN = false;

            return await Task.FromResult(true);
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Manage
{
    public class ActivateEventTypeCommand : IRequest<bool>
    {
        public readonly Guid eventTypeId;

        public ActivateEventTypeCommand(Guid eventTypeId)
        {
            this.eventTypeId = eventTypeId;
        }
    }

    public class ActivateEventTypeCommandHandler : IRequestHandler<ActivateEventTypeCommand, bool>
    {
        private readonly IEventTypeRepository eventTypeRepository;

        public ActivateEventTypeCommandHandler(IEventTypeRepository eventTypeRepository)
        {
            this.eventTypeRepository = eventTypeRepository;
        }
        public async Task<bool> Handle(ActivateEventTypeCommand request, CancellationToken cancellationToken)
        {
            var eventType = await eventTypeRepository.GetEventTypeById(request.eventTypeId);

            if (eventType == null)
                throw new Exception("Not found.");

            if (eventType.ActiveYN)
                throw new Exception("Already activated.");

            eventType.ActiveYN = true;

            await eventTypeRepository.UpdateEventType(eventType);

            return await Task.FromResult(true);
        }

    }

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

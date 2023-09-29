using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Commands
{
    public class ToggleEventTypeStatusCommand : IRequest<bool>
    {
        public readonly Guid eventTypeId;

        public ToggleEventTypeStatusCommand(Guid eventTypeId)
        {
            this.eventTypeId = eventTypeId;
        }
    }

    public class ToggleEventTypeStatusCommandHandler : IRequestHandler<ToggleEventTypeStatusCommand, bool>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public ToggleEventTypeStatusCommandHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }
        public async Task<bool> Handle(ToggleEventTypeStatusCommand request, CancellationToken cancellationToken)
        {
            var eventType = await persistenceProvider.GetEventTypeById(request.eventTypeId);

            if (eventType == null)
            {
                throw new Exception("Not found.");
            }
            eventType.ActiveYN = !eventType.ActiveYN;

            await persistenceProvider.UpdateEventType(eventType);

            return await Task.FromResult(true);
        }

    }


}

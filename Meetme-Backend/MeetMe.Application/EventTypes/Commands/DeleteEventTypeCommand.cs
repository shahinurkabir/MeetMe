using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Exceptions;

namespace MeetMe.Application.EventTypes.Commands
{
    public class DeleteEventTypeCommand : IRequest<bool>
    {
        public readonly Guid eventTypeId;
        public DeleteEventTypeCommand(Guid eventTypeId)
        {
            this.eventTypeId = eventTypeId;
        }
    }

    public class DeleteEventTypeCommandHandler : IRequestHandler<DeleteEventTypeCommand, bool>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public DeleteEventTypeCommandHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }

        public async Task<bool> Handle(DeleteEventTypeCommand request, CancellationToken cancellationToken)
        {
            var eventType = await persistenceProvider.GetEventTypeById(request.eventTypeId);

            if (eventType == null)
            {
                throw new MeetMeException("Not found.");
            }
            if (eventType.IsDeleted)
            {
                throw new MeetMeException("Already deleted.");
            }

            eventType.IsDeleted = true;

            await persistenceProvider.UpdateEventType(eventType);

            return await Task.FromResult(true);
        }

    }

}

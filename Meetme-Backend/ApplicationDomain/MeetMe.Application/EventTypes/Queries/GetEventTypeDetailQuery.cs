using MeetMe.Core.Interface;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Interface;
using System;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.EventTypes.Queries
{
    public class GetEventTypeDetailQuery : IRequest<EventType>
    {
        public Guid EventTypeId { get; set; }
    }

    public class EventTypeDetailQueryHandler : IRequestHandler<GetEventTypeDetailQuery, EventType>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public EventTypeDetailQueryHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }
        public async Task<EventType> Handle(GetEventTypeDetailQuery request, CancellationToken cancellationToken)
        {
            var eventType = await persistenceProvider.GetEventTypeById(request.EventTypeId);

            return eventType;
        }
    }

}

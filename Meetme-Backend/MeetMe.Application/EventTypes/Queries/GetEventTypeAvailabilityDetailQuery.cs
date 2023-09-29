using MeetMe.Core.Persistence.Entities;
using MediatR;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Queries
{
    public class GetEventTypeAvailabilityQuery : IRequest<List<EventTypeAvailabilityDetail>>
    {
        public Guid EvnetTypeId { get; set; }
    };

    public class GetEventTypeAvailabilityQueryHandler : IRequestHandler<GetEventTypeAvailabilityQuery, List<EventTypeAvailabilityDetail>>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public GetEventTypeAvailabilityQueryHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }
        public async Task<List<EventTypeAvailabilityDetail>> Handle(GetEventTypeAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var eventTypeEntity = await persistenceProvider.GetEventTypeById(request.EvnetTypeId);
            return eventTypeEntity.EventTypeAvailabilityDetails;
        }
    }
}

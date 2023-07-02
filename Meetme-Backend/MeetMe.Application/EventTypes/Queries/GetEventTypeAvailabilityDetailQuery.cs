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
        private readonly IEventTypeAvailabilityRepository eventTypeAvailabilityDetailRepository;

        public GetEventTypeAvailabilityQueryHandler(IEventTypeAvailabilityRepository  eventTypeAvailabilityDetailRepository)
        {
            this.eventTypeAvailabilityDetailRepository = eventTypeAvailabilityDetailRepository;
        }
        public async Task<List<EventTypeAvailabilityDetail>> Handle(GetEventTypeAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var availability = await eventTypeAvailabilityDetailRepository.GetEventTypeAvailabilityByEventId(request.EvnetTypeId);
            return availability;
        }
    }
}

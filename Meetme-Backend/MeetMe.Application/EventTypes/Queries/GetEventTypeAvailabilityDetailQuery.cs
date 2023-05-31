using MeetMe.Core.Persistence.Entities;
using MediatR;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Queries
{
    public class GetEventTypeAvailabilityDetailQuery : IRequest<List<EventTypeAvailabilityDetail>>
    {
        public Guid EvnetTypeId { get; set; }
    };

    public class GetEventTypeAvailabilityDetailQueryHandler : IRequestHandler<GetEventTypeAvailabilityDetailQuery, List<EventTypeAvailabilityDetail>>
    {
        private readonly IEventTypeAvailabilityDetailRepository eventTypeAvailabilityDetailRepository;

        public GetEventTypeAvailabilityDetailQueryHandler(IEventTypeAvailabilityDetailRepository  eventTypeAvailabilityDetailRepository)
        {
            this.eventTypeAvailabilityDetailRepository = eventTypeAvailabilityDetailRepository;
        }
        public async Task<List<EventTypeAvailabilityDetail>> Handle(GetEventTypeAvailabilityDetailQuery request, CancellationToken cancellationToken)
        {
            var availability = await eventTypeAvailabilityDetailRepository.GetEventTypeAvailabilityDetailByEventId(request.EvnetTypeId);
            return availability;
        }
    }
}

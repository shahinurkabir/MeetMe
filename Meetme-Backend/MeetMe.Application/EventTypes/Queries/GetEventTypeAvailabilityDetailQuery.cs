using MeetMe.Core.Persistence.Entities;
using MediatR;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Queries
{
    public class GetEventTypeAvailabilityDetailQuery : IRequest<EventTypeAvailability>
    {
        public Guid EvnetTypeId { get; set; }
    };

    public class GetEventTypeAvailabilityDetailQueryHandler : IRequestHandler<GetEventTypeAvailabilityDetailQuery, EventTypeAvailability>
    {
        private readonly IEventAvailabilityRepository eventAvailabilityRepository;

        public GetEventTypeAvailabilityDetailQueryHandler(IEventAvailabilityRepository  eventAvailabilityRepository)
        {
            this.eventAvailabilityRepository = eventAvailabilityRepository;
        }
        public async Task<EventTypeAvailability> Handle(GetEventTypeAvailabilityDetailQuery request, CancellationToken cancellationToken)
        {
            var availability = await eventAvailabilityRepository.GetEventAvailabilityById(request.EvnetTypeId);
            return availability;
        }
    }
}

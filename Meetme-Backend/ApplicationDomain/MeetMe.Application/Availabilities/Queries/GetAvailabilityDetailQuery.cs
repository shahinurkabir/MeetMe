using MediatR;
using System;
using MeetMe.Core.Persistence.Entities;
using System.Threading;
using MeetMe.Core.Persistence.Interface;
using System.Threading.Tasks;

namespace MeetMe.Application.Availabilities.Queries
{
    public class GetAvailabilityDetailQuery : IRequest<Availability>
    {
        public Guid Id { get; set; }
    }

    public class GetAvailabilityDetailQueryHandler : IRequestHandler<GetAvailabilityDetailQuery, Availability>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public GetAvailabilityDetailQueryHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }
        public async Task<Availability> Handle(GetAvailabilityDetailQuery request, CancellationToken cancellationToken)
        {
            var availability = await persistenceProvider.GetAvailability(request.Id);

            return availability;
        }
    }
}

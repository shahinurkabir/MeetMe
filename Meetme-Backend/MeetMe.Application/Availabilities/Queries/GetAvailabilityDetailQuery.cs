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
        private readonly IAvailabilityRepository _availabilityRepository;

        public GetAvailabilityDetailQueryHandler(IAvailabilityRepository availabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
        }
        public async Task<Availability> Handle(GetAvailabilityDetailQuery request, CancellationToken cancellationToken)
        {
            var availability = await _availabilityRepository.GetAvailability(request.Id);

            return availability;
        }
    }
}

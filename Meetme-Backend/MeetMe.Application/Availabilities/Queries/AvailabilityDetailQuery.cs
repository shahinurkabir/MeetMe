using MediatR;
using System;
using MeetMe.Core.Persistence.Entities;
using System.Threading;
using MeetMe.Core.Persistence.Interface;
using System.Threading.Tasks;

namespace MeetMe.Application.Availabilities.Queries
{
    public class AvailabilityDetailQuery : IRequest<Availability>
    {
        public Guid Id { get; set; }
    }

    public class ScheduleDetailQueryHandler : IRequestHandler<AvailabilityDetailQuery, Availability>
    {
        private readonly IAvailabilityRepository availabilityRepository;

        public ScheduleDetailQueryHandler(IAvailabilityRepository availabilityRepository)
        {
            this.availabilityRepository = availabilityRepository;
        }
        public async Task<Availability> Handle(AvailabilityDetailQuery request, CancellationToken cancellationToken)
        {
            var ruleInfo = await availabilityRepository.GetById(request.Id);

            return ruleInfo;
        }
    }
}

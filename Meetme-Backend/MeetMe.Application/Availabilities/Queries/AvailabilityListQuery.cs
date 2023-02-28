using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Availabilities.Queries
{
    public class AvailabilityListQuery : IRequest<List<Availability>>
    {
        public Guid UserId { get; set; }
    }
    public class ScheduleListQueryHandler : IRequestHandler<AvailabilityListQuery, List<Availability>>
    {
        private readonly IAvailabilityRepository availabilityRepository;

        public ScheduleListQueryHandler(IAvailabilityRepository availabilityRepository)
        {
            this.availabilityRepository = availabilityRepository;
        }
        public async Task<List<Availability>> Handle(AvailabilityListQuery request, CancellationToken cancellationToken)
        {
            var list = await availabilityRepository.GetScheduleListByUserId(request.UserId);

            return list;
        }
    }


}

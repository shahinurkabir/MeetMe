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
    public class GetAvailabilityListQuery : IRequest<List<Availability>>
    {
        public Guid UserId { get; set; }
    }
    public class GetAvailabilityListQueryHandler : IRequestHandler<GetAvailabilityListQuery, List<Availability>>
    {
        private readonly IAvailabilityRepository _availabilityRepository;

        public GetAvailabilityListQueryHandler(IAvailabilityRepository availabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
        }
        public async Task<List<Availability>> Handle(GetAvailabilityListQuery request, CancellationToken cancellationToken)
        {
            var listOfAvailability = await _availabilityRepository.GetListByUserId(request.UserId);

            return listOfAvailability;
        }
    }


}

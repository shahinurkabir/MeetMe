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
        private readonly IPersistenceProvider persistenceProvider;

        public GetAvailabilityListQueryHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }
        public async Task<List<Availability>> Handle(GetAvailabilityListQuery request, CancellationToken cancellationToken)
        {
            var listOfAvailability = await persistenceProvider.GetListByUserId(request.UserId);

            return listOfAvailability;
        }
    }


}

using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Interface;
using System;

namespace MeetMe.Application.EventTypes.Queries
{
    public class GetEventTypeListQuery : IRequest<List<EventType>>
    {
        public Guid OwnerId { get; set; }
    };

    public class GetEventTypeListQueryHandler : IRequestHandler<GetEventTypeListQuery, List<EventType>>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public GetEventTypeListQueryHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }
        public async Task<List<EventType>> Handle(GetEventTypeListQuery request, CancellationToken cancellationToken)
        {
            var list = await persistenceProvider.GetEventTypeListByUserId(request.OwnerId);
            return list;
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.ScheduleRules.Queries
{
    public class ScheduleListQuery : IRequest<List<ScheduleRule>>
    {
        public Guid UserId { get; set; }
    }
    public class ScheduleListQueryHandler : IRequestHandler<ScheduleListQuery, List<ScheduleRule>>
    {
        private readonly IPersistenceProvider persistenseProvider;

        public ScheduleListQueryHandler(IPersistenceProvider persistenseProvider)
        {
            this.persistenseProvider = persistenseProvider;
        }
        public async Task<List<ScheduleRule>> Handle(ScheduleListQuery request, CancellationToken cancellationToken)
        {
            var list = await persistenseProvider.GetScheduleListByUserId(request.UserId);

            return list;
        }
    }
    

}

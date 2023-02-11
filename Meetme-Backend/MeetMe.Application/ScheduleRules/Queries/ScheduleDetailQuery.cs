using MediatR;
using System;
using MeetMe.Core.Persistence.Entities;
using System.Threading;
using MeetMe.Core.Persistence.Interface;
using System.Threading.Tasks;

namespace MeetMe.Application.ScheduleRules.Queries
{
    public class ScheduleDetailQuery : IRequest<ScheduleRule>
    {
        public Guid Id { get; set; }
    }

    public class ScheduleDetailQueryHandler : IRequestHandler<ScheduleDetailQuery, ScheduleRule>
    {
        private readonly IPersistenceProvider persistenseProvider;

        public ScheduleDetailQueryHandler(IPersistenceProvider persistenseProvider)
        {
            this.persistenseProvider = persistenseProvider;
        }
        public async Task<ScheduleRule> Handle(ScheduleDetailQuery request, CancellationToken cancellationToken)
        {
            var ruleInfo = await persistenseProvider.GetScheduleById(request.Id);

            return ruleInfo;
        }
    }
}

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;

namespace MeetMe.Application.ScheduleRules.Commands.Clone
{
    public class CloneAppointmentScheduleCommandHandler : IRequestHandler<CloneAppointmentScheduleCommand, Guid>
    {
        private readonly IPersistenceProvider persistenseProvider;
        private readonly IUserInfo applicationUserInfo;

        public CloneAppointmentScheduleCommandHandler(IPersistenceProvider persistenseProvider, IUserInfo applicationUserInfo)
        {
            this.persistenseProvider = persistenseProvider;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CloneAppointmentScheduleCommand request, CancellationToken cancellationToken)
        {
            var newscheduleRule = Guid.NewGuid();

            var originalModel = await persistenseProvider.GetScheduleById(request.AppointmentScheduleId);

            var cloneName = $"Clone of {originalModel.Name }";

            var cloneModel = new ScheduleRule
            {
                Id = newscheduleRule,
                Name = cloneName,
                OwnerId=applicationUserInfo.UserId,
                RuleAttributes = originalModel.RuleAttributes
            };

            _ = await persistenseProvider.AddSchedule(cloneModel);

            return newscheduleRule;
        }
    }
}

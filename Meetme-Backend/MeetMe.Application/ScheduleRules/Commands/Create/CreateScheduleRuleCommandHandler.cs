using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;
using MeetMe.Application.ScheduleRules.Extensions;

namespace MeetMe.Application.ScheduleRules.Commands
{
    public class CreateScheduleRuleCommandHandler : IRequestHandler<CreateScheduleRuleCommand, Guid>
    {
        private readonly IPersistenceProvider persistenseProvider;
        private readonly IUserInfo applicationUserInfo;

        public CreateScheduleRuleCommandHandler(IPersistenceProvider persistenseProvider, IUserInfo applicationUserInfo)
        {
            this.persistenseProvider = persistenseProvider;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CreateScheduleRuleCommand request, CancellationToken cancellationToken)
        {
            var scheduleRuleId = Guid.NewGuid();

            var entity = new ScheduleRule
            {
                Id = scheduleRuleId,
                Name = request.Name,
                OwnerId = applicationUserInfo.UserId,
                TimeZoneId = request.TimeZoneId
            };

            entity.RuleAttributes = request.RuleItems.ToEntityList(entity.Id);

            _ = await persistenseProvider.AddSchedule(entity);


            return scheduleRuleId;

        }

        private ScheduleRule ConvertToEntity(CreateScheduleRuleCommand command, Guid appointmentScheduleId, Guid ownerId)
        {
            var appointmentSchedule = new ScheduleRule
            {
                Id = appointmentScheduleId,
                Name = command.Name,
                OwnerId = ownerId,
                TimeZoneId = command.TimeZoneId
            };

            int itemId = 0;

            command.RuleItems.ForEach(e =>
            {
                var item = new ScheduleRuleAttribute
                {
                    Id = itemId,
                    RuleId = appointmentScheduleId,
                    Type = e.Type,
                    Day = e.Day,
                    Date = e.Date,
                    From = e.From,
                    To = e.To
                };

                appointmentSchedule.RuleAttributes.Add(item);

                itemId++;

            });

            return appointmentSchedule;

        }
    }
}

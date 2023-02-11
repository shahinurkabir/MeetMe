using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;
using System.Collections.Generic;
using MeetMe.Application.ScheduleRules.Extensions;

namespace MeetMe.Application.ScheduleRules.Commands.Update
{
    public class UpdateScheduleRuleCommandHandler : IRequestHandler<UpdateScheduleRuleCommand, bool>
    {
        private readonly IPersistenceProvider persistenseProvider;
        private readonly IUserInfo applicationUserInfo;

        public UpdateScheduleRuleCommandHandler(IPersistenceProvider persistenseProvider, IUserInfo applicationUserInfo)
        {
            this.persistenseProvider = persistenseProvider;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<bool> Handle(UpdateScheduleRuleCommand request, CancellationToken cancellationToken)
        {

            var entity = await persistenseProvider.GetScheduleById(request.Id);

            if (entity == null) throw new MeetMe.Core.Exceptions.CustomException("Invalid schedule provided.");

            var listScheduleRuleItems = request.RuleItems.ToEntityList(entity.Id);// GetScheduleRuleItems(request, entity.Id);

            entity.Name = request.Name;
            entity.RuleAttributes = listScheduleRuleItems;


            _ = await persistenseProvider.UpdateSchedule(entity);

            return true;

        }

        private List<ScheduleRuleAttribute> GetScheduleRuleItems(UpdateScheduleRuleCommand command, Guid scheduleRuleId)
        {

            var listScheduleRuleItems = new List<ScheduleRuleAttribute>();

            int itemId = 0;

            command.RuleItems.ForEach(e =>
            {
                var item = new ScheduleRuleAttribute
                {
                    Id = itemId,
                    RuleId = scheduleRuleId,
                    Type = e.Type,
                    Day = e.Day,
                    Date = e.Date,
                    From = e.From,
                    To = e.To
                };

                listScheduleRuleItems.Add(item);

                itemId++;

            });

            return listScheduleRuleItems;

        }
    }
}

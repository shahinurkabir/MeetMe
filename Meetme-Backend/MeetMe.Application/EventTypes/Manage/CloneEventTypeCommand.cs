using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Persistence.Entities;

namespace MeetMe.Application.EventTypes.Manage
{
    public class CloneEventTypeCommand : IRequest<Guid>
    {
        public readonly Guid eventTypeId;

        public CloneEventTypeCommand(Guid eventTypeId)
        {
            this.eventTypeId = eventTypeId;
        }
    }

    public class CloneEventTypeCommandHandler : IRequestHandler<CloneEventTypeCommand, Guid>
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly IDateTimeService dateTimeService;
        private readonly IUserInfo applicationUserInfo;

        public CloneEventTypeCommandHandler(
            IEventTypeRepository eventTypeRepository,
            IDateTimeService dateTimeService,
            IUserInfo applicationUserInfo
            )
        {
            this.eventTypeRepository = eventTypeRepository;
            this.dateTimeService = dateTimeService;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CloneEventTypeCommand request, CancellationToken cancellationToken)
        {
            var eventType = await eventTypeRepository.GetEventTypeById(request.eventTypeId);

            if (eventType == null)
                throw new Exception("Not found.");

            var newEventTypeId = Guid.NewGuid();

            var cloneEventTypeEntity=CloneEventType(eventType, newEventTypeId);

            await eventTypeRepository.AddNewEventType(cloneEventTypeEntity);

            return newEventTypeId;
        }

        private EventType CloneEventType(EventType eventType, Guid newEventTypeId)
        {
            var cloneEventType = new EventType
            {
                Id = newEventTypeId,
                Name = $"{eventType.Name} (clone)" ,
                Slug =$"{eventType.Name}-clone",
                ActiveYN = eventType.ActiveYN,
                OwnerId = eventType.OwnerId,
                AvailabilityId = eventType.AvailabilityId,
                IsDeleted = false,
                BufferTimeAfter = eventType.BufferTimeAfter,
                BufferTimeBefore = eventType.BufferTimeBefore,
                DateForwardKind = eventType.DateForwardKind,
                EventColor = eventType.EventColor,
                Duration = eventType.Duration,
                Description = eventType.Description,
                ForwardDuration = eventType.ForwardDuration,
                Location = eventType.Location,
                TimeZone = eventType.TimeZone,
                DateFrom = eventType.DateFrom,
                DateTo = eventType.DateTo,
                CreatedAt = dateTimeService.GetCurrentTime,
                CreatedBy = applicationUserInfo.Id,
                UpdatedAt = dateTimeService.GetCurrentTimeUtc,

                EventTypeAvailabilityDetails = eventType.EventTypeAvailabilityDetails.Select(e =>
                {
                    return new EventTypeAvailabilityDetail
                    {
                        EventTypeId = eventType.Id,
                        DayType = e.DayType,
                        Value = e.Value,
                        From = e.From,
                        To = e.To,
                        StepId = e.StepId
                    };
                }).ToList(),

                Questions = eventType.Questions.Select(e =>
                {
                    return new MeetMe.Core.Persistence.Entities.EventTypeQuestion
                    {
                        Id = Guid.NewGuid(),
                        EventTypeId = newEventTypeId,
                        ActiveYN = e.ActiveYN,
                        Name = e.Name,
                        DisplayOrder = e.DisplayOrder,
                        QuestionType = e.QuestionType,
                        RequiredYN = e.RequiredYN,
                        Options = e.Options,
                        OtherOptionYN = e.OtherOptionYN,
                    };
                }).ToList()

            };

            return cloneEventType;
        }
    }


}

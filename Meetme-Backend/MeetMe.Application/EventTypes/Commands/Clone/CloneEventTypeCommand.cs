﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Exceptions;

namespace MeetMe.Application.EventTypes.Commands.Clone
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
        private readonly IEventTypeRepository _eventTypeRepository;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILoginUserInfo _applicationUserInfo;

        public CloneEventTypeCommandHandler(IEventTypeRepository eventTypeRepository, IDateTimeService dateTimeService, ILoginUserInfo applicationUserInfo)
        {
            _eventTypeRepository = eventTypeRepository;
            _dateTimeService = dateTimeService;
            _applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CloneEventTypeCommand request, CancellationToken cancellationToken)
        {
            var eventType = await _eventTypeRepository.GetEventTypeById(request.eventTypeId);

            if (eventType == null)
            {
                throw new MeetMeException("Not found.");
            }
            var newEventTypeId = Guid.NewGuid();

            var newSlug = await GetUniqueSlug(eventType.Slug);

            var cloneEventTypeEntity = CloneEventType(eventType, newSlug, newEventTypeId);

            await _eventTypeRepository.AddNewEventType(cloneEventTypeEntity);

            return newEventTypeId;
        }

        private EventType CloneEventType(EventType eventType, string newSlug, Guid newEventTypeId)
        {
            var cloneEventType = new EventType
            {
                Id = newEventTypeId,
                Name = $"{eventType.Name} (clone)",
                Slug = newSlug,
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
                CreatedAt = _dateTimeService.GetCurrentTime,
                CreatedBy = _applicationUserInfo.Id,
                UpdatedAt = _dateTimeService.GetCurrentTimeUtc,
            };

            cloneEventType.EventTypeAvailabilityDetails.AddRange(eventType.EventTypeAvailabilityDetails.Select(e =>
                    new EventTypeAvailabilityDetail
                    {
                        EventTypeId = eventType.Id,
                        DayType = e.DayType,
                        Value = e.Value,
                        From = e.From,
                        To = e.To,
                        StepId = e.StepId
                    }));

            cloneEventType.Questions.AddRange(eventType.Questions.Select(e =>
                    new EventTypeQuestion
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
                        SystemDefinedYN = e.SystemDefinedYN
                    }));

            return cloneEventType;
        }

        private async Task<string> GetUniqueSlug(string slug)
        {
            var newSlug = $"{slug}-clone";

            var listEventForThisUser = await _eventTypeRepository.GetEventTypeListByUserId(_applicationUserInfo.Id);

            if (!listEventForThisUser.Any())
            {
                return newSlug;
            }

            var index = 1;

            while (listEventForThisUser.Any(e => e.Slug == newSlug))
            {
                newSlug = $"{slug}-clone-{index}";
                index++;
            }

            return newSlug;
        }
    }


}

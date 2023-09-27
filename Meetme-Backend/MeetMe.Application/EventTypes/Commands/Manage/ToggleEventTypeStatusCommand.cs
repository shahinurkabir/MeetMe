﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Commands.Manage
{
    public class ToggleEventTypeStatusCommand : IRequest<bool>
    {
        public readonly Guid eventTypeId;

        public ToggleEventTypeStatusCommand(Guid eventTypeId)
        {
            this.eventTypeId = eventTypeId;
        }
    }

    public class ToggleEventTypeStatusCommandHandler : IRequestHandler<ToggleEventTypeStatusCommand, bool>
    {
        private readonly IEventTypeRepository _eventTypeRepository;

        public ToggleEventTypeStatusCommandHandler(IEventTypeRepository eventTypeRepository)
        {
            _eventTypeRepository = eventTypeRepository;
        }
        public async Task<bool> Handle(ToggleEventTypeStatusCommand request, CancellationToken cancellationToken)
        {
            var eventType = await _eventTypeRepository.GetEventTypeById(request.eventTypeId);

            if (eventType == null)
            {
                throw new Exception("Not found.");
            }
            eventType.ActiveYN = !eventType.ActiveYN;

            await _eventTypeRepository.UpdateEventType(eventType);

            return await Task.FromResult(true);
        }

    }


}

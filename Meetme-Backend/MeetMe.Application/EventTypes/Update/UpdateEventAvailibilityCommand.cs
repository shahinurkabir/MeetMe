using MediatR;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MeetMe.Application.EventTypes.Update
{
    public class UpdateEventAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string DateForwardKind { get; set; } = null!;
        public int? ForwardDuration { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Duration { get; set; }
        public int BufferTimeBefore { get; set; }
        public int BufferTimeAfter { get; set; }
        public int TimeZoneId { get; set; }
        public Guid? AvailabilityId { get; set; }

        public List<AvailabilityDetail>? CustomAvailabilityDetails { get; set; }
    }

    public class UpdateAvailabilityCommandHandler : IRequestHandler<UpdateEventAvailabilityCommand, bool>
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly IUserInfo loginUser;

        public UpdateAvailabilityCommandHandler(
            IEventTypeRepository eventTypeRepository,
            IAvailabilityRepository availabilityRepository,
            IUserInfo loginUser

            )
        {
            this.eventTypeRepository = eventTypeRepository;
            this.availabilityRepository = availabilityRepository;
            this.loginUser = loginUser;
        }
        public async Task<bool> Handle(UpdateEventAvailabilityCommand request, CancellationToken cancellationToken)
        {

                var eventTypeEntity = await eventTypeRepository.GetEventTypeById(request.Id);
                if (eventTypeEntity == null) throw new CustomException("Event Type is not found.");

                if (request.CustomAvailabilityDetails != null &&
                    request.CustomAvailabilityDetails.Any()
                 )
                {
                    var customAvailabilityId = Guid.NewGuid();

                    await CreateCustomAvailabilityEntity(request, customAvailabilityId);

                    request.AvailabilityId = customAvailabilityId;

                }

                await UpdateEventTypeFields(eventTypeEntity, request);

                return await Task.FromResult(true);

        }

        private async Task CreateCustomAvailabilityEntity(UpdateEventAvailabilityCommand request, Guid customAvailabilityId)
        {
            request.CustomAvailabilityDetails?.ForEach(e => e.AvailabilityId = customAvailabilityId);

            var availability = new Availability
            {
                Id = customAvailabilityId,
                Name = "Custom availability for EventType " + request.Id,
                OwnerId = loginUser.UserId,
                TimeZoneId = request.TimeZoneId,
                IsDefault = false,
                IsCustom = true,
                Details = request.CustomAvailabilityDetails?.ToList()
            };

            _ = await availabilityRepository.AddSchedule(availability);

        }
       
        private async Task UpdateEventTypeFields(EventType eventType, UpdateEventAvailabilityCommand request)
        {

            eventType.DateForwardKind = request.DateForwardKind;
            eventType.ForwardDuration = request.ForwardDuration;
            eventType.Duration = request.Duration;
            eventType.DateFrom = request.DateFrom;
            eventType.DateTo = request.DateTo;
            eventType.BufferTimeBefore = request.BufferTimeBefore;
            eventType.BufferTimeAfter = request.BufferTimeAfter;
            eventType.TimeZoneId = request.TimeZoneId;
            eventType.AvailabilityId = request.AvailabilityId;

            await eventTypeRepository.UpdateEventType(eventType);
        }

    }


}

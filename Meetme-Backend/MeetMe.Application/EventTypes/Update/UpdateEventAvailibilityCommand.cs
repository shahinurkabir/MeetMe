using MediatR;
using MeetMe.Core.Exceptions;
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

        public List<AvailabilityDetailItem>? AvailabilityDetails { get; set; }
    }

    public class AvailabilityDetailItem
    {
        /// <summary>
        /// D:Date
        /// W:Weekday
        /// </summary>
        public string DayType { get; set; } = null!;
        public string? Value { get; set; }
        public short StepId { get; set; }
        public double From { get; set; }
        public double To { get; set; }

    }
    public class UpdateAvailabilityCommandHandler : IRequestHandler<UpdateEventAvailabilityCommand, bool>
    {
        private readonly IEventTypeRepository eventTypeRepository;

        public UpdateAvailabilityCommandHandler(IEventTypeRepository eventTypeRepository)
        {
            this.eventTypeRepository = eventTypeRepository;
        }
        public async Task<bool> Handle(UpdateEventAvailabilityCommand request, CancellationToken cancellationToken)
        {

            var eventType = await eventTypeRepository.GetEventTypeById(request.Id);
            if (eventType == null) throw new CustomException("Event Type is not found.");

            UpdateFields(eventType, request);

            await eventTypeRepository.UpdateEventType(eventType);

            return await Task.FromResult(true);
        }

        private static void UpdateFields(EventType eventType, UpdateEventAvailabilityCommand request)
        {

            eventType.DateForwardKind = request.DateForwardKind;
            eventType.ForwardDuration = request.ForwardDuration;
            eventType.Duration = request.Duration;
            eventType.DateFrom = request.DateFrom;
            eventType.DateTo = request.DateTo;
            eventType.BufferTimeBefore = request.BufferTimeBefore;
            eventType.BufferTimeAfter = request.BufferTimeAfter;
            eventType.TimeZoneId = request.TimeZoneId;
            eventType.CustomAvailability = CustomAvailabilityData(request.AvailabilityDetails);

        }
        private static string? CustomAvailabilityData(List<AvailabilityDetailItem>? eventTypeAvailabilityDetails)
        {
            if (eventTypeAvailabilityDetails == null || !eventTypeAvailabilityDetails.Any()) return null;

            string jsonString = JsonSerializer.Serialize(eventTypeAvailabilityDetails);
            return jsonString;
        }

    }


}

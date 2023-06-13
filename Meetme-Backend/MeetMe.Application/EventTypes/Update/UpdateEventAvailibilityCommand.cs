using MediatR;
using MeetMe.Application.EventTypes.Dtos;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;

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

        public List<EventAvailabilityDetailItemDto> AvailabilityDetails { get; set; } = null!;


    }

    public class UpdateAvailabilityCommandHandler : IRequestHandler<UpdateEventAvailabilityCommand, bool>
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly IEventTypeAvailabilityDetailRepository eventTypeAvailabilityDetailRepository;
        private readonly IUserInfo loginUser;

        public UpdateAvailabilityCommandHandler
        (
            IEventTypeRepository eventTypeRepository,
            IEventTypeAvailabilityDetailRepository eventTypeAvailabilityDetailRepository,
            IUserInfo loginUser

        )
        {
            this.eventTypeRepository = eventTypeRepository;
            this.eventTypeAvailabilityDetailRepository = eventTypeAvailabilityDetailRepository;
            this.loginUser = loginUser;
        }

        public async Task<bool> Handle(UpdateEventAvailabilityCommand request, CancellationToken cancellationToken)
        {

            var eventTypeEntity = await eventTypeRepository.GetEventTypeById(request.Id);

            if (eventTypeEntity == null) throw new CustomException("Event Type is not found.");

            var listScheduleItem = await eventTypeAvailabilityDetailRepository.GetEventTypeAvailabilityDetailByEventId(eventTypeEntity.Id);

            if (listScheduleItem != null && listScheduleItem.Any())
            {
                await eventTypeAvailabilityDetailRepository.RemoveItems(listScheduleItem);
            }

            await UpdateEventTypeFields(eventTypeEntity, request);

            await UpdateEventAvailabilityDetails(eventTypeEntity.Id, request.AvailabilityDetails);

            return await Task.FromResult(true);

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

        private async Task UpdateEventAvailabilityDetails(Guid eventTypeId, List<EventAvailabilityDetailItemDto> availabilityDetails)
        {
            var eventAvailabilityList = availabilityDetails.Select(e => new EventTypeAvailabilityDetail
            {
                Id = Guid.NewGuid(),
                EventTypeId = eventTypeId,
                DayType = e.DayType,
                Value = e.Value,
                From = e.From,
                To = e.To,
                StepId = e.StepId
            }).ToList();

            await eventTypeAvailabilityDetailRepository.InsertItems(eventAvailabilityList);


        }

    }


}

using MediatR;
using MeetMe.Application.EventTypes.Dtos;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Commands.Update
{
    public class UpdateEventAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string DateForwardKind { get; set; } = null!;
        public int? ForwardDuration { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Duration { get; set; }
        public int BufferTimeBefore { get; set; } = 0;
        public int BufferTimeAfter { get; set; } = 0;
        public string TimeZone { get; set; } = null!;
        public Guid? AvailabilityId { get; set; }

        public List<EventAvailabilityDetailItemDto> AvailabilityDetails { get; set; } = null!;


    }

    public class UpdateAvailabilityCommandHandler : IRequestHandler<UpdateEventAvailabilityCommand, bool>
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly IEventTypeAvailabilityRepository eventTypeAvailabilityDetailRepository;
        private readonly ILoginUserInfo loginUser;

        public UpdateAvailabilityCommandHandler
        (
            IEventTypeRepository eventTypeRepository,
            IEventTypeAvailabilityRepository eventTypeAvailabilityDetailRepository,
            ILoginUserInfo loginUser

        )
        {
            this.eventTypeRepository = eventTypeRepository;
            this.eventTypeAvailabilityDetailRepository = eventTypeAvailabilityDetailRepository;
            this.loginUser = loginUser;
        }

        public async Task<bool> Handle(UpdateEventAvailabilityCommand request, CancellationToken cancellationToken)
        {

            var eventTypeEntity = await eventTypeRepository.GetEventTypeById(request.Id);

            if (eventTypeEntity == null) throw new MeetMeException("Event Type is not found.");

            var listScheduleItem = await eventTypeAvailabilityDetailRepository.GetEventTypeAvailabilityByEventId(eventTypeEntity.Id);

            if (listScheduleItem != null && listScheduleItem.Any())
            {
                await eventTypeAvailabilityDetailRepository.RemoveItems(listScheduleItem);
            }

            UpdateEventTypeFields(eventTypeEntity, request);

            var eventAvailabilityList = ConvertToAvailabilityDetails(eventTypeEntity.Id, request.AvailabilityDetails);

            await eventTypeRepository.UpdateEventAvailability(eventTypeEntity, eventAvailabilityList);

            //await UpdateEventTypeFields(eventTypeEntity, request);

            //await UpdateEventAvailabilityDetails(eventTypeEntity.Id, request.AvailabilityDetails);

            return await Task.FromResult(true);

        }

        private void UpdateEventTypeFields(EventType eventType, UpdateEventAvailabilityCommand request)
        {

            eventType.DateForwardKind = request.DateForwardKind;
            eventType.ForwardDuration = request.ForwardDuration;
            eventType.Duration = request.Duration;
            eventType.DateFrom = request.DateFrom;
            eventType.DateTo = request.DateTo;
            eventType.BufferTimeBefore = request.BufferTimeBefore;
            eventType.BufferTimeAfter = request.BufferTimeAfter;
            eventType.TimeZone = request.TimeZone;
            eventType.AvailabilityId = request.AvailabilityId;

            //await eventTypeRepository.UpdateEventType(eventType);
        }

        private List<EventTypeAvailabilityDetail> ConvertToAvailabilityDetails(Guid eventTypeId, List<EventAvailabilityDetailItemDto> availabilityDetails)
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

            return eventAvailabilityList;
            //await eventTypeAvailabilityDetailRepository.InsertItems(eventAvailabilityList);


        }

    }


}

using MediatR;
using MeetMe.Application.EventTypes.Dtos;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.EventTypes.Commands
{
    public class UpdateEventAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string DateForwardKind { get; set; } = null!;
        public int ForwardDuration { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int BufferTimeBefore { get; set; } = 0;
        public int BufferTimeAfter { get; set; } = 0;
        public string TimeZone { get; set; } = null!;
        public Guid? AvailabilityId { get; set; }

        public List<EventAvailabilityDetailItemDto> AvailabilityDetails { get; set; } = null!;

    }

    public class UpdateAvailabilityCommandHandler : IRequestHandler<UpdateEventAvailabilityCommand, bool>
    {
        private readonly IPersistenceProvider persistenceProvider;
        private readonly ILoginUserInfo _loginUser;

        public UpdateAvailabilityCommandHandler(IPersistenceProvider persistenceProvider, ILoginUserInfo loginUser)
        {
            this.persistenceProvider = persistenceProvider;
            _loginUser = loginUser;
        }
        public async Task<bool> Handle(UpdateEventAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var eventTypeEntity = await persistenceProvider.GetEventTypeById(request.Id);

            if (eventTypeEntity == null)
            {
                throw new MeetMeException("Event Type is not found.");
            }

            eventTypeEntity = MapCommandToEntity(eventTypeEntity, request);

            await persistenceProvider.UpdateEventAvailability(eventTypeEntity);

            return true;
        }
        private EventType MapCommandToEntity(EventType entityTypeExisting, UpdateEventAvailabilityCommand request)
        {
            entityTypeExisting.DateForwardKind = request.DateForwardKind;
            entityTypeExisting.ForwardDuration = request.ForwardDuration;
            entityTypeExisting.DateFrom = request.DateFrom;
            entityTypeExisting.DateTo = request.DateTo;
            entityTypeExisting.BufferTimeBefore = request.BufferTimeBefore;
            entityTypeExisting.BufferTimeAfter = request.BufferTimeAfter;
            entityTypeExisting.TimeZone = request.TimeZone;
            entityTypeExisting.AvailabilityId = request.AvailabilityId;

            entityTypeExisting.EventTypeAvailabilityDetails = MapCommandToEntity( request);

            return entityTypeExisting;

        }
        private List<EventTypeAvailabilityDetail> MapCommandToEntity( UpdateEventAvailabilityCommand request)
        {
            return request.AvailabilityDetails.Select(e => new EventTypeAvailabilityDetail
            {
                EventTypeId = request.Id,
                DayType = e.DayType,
                Value = e.Value,
                From = e.From,
                To = e.To,
                StepId = e.StepId
            }).ToList();
        }
    }
}

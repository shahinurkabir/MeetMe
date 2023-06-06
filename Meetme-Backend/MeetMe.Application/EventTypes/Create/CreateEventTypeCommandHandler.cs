using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MediatR;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Constant;

namespace MeetMe.Application.EventTypes.Create
{
    public class CreateEventTypeCommandHandler : IRequestHandler<CreateEventTypeCommand, Guid>
    {
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly ITimeZoneDataRepository timeZoneDataRepository;
        private readonly IUserInfo applicationUser;
        private readonly IDateTimeService dateTimeService;

        public CreateEventTypeCommandHandler(
            IAvailabilityRepository availabilityRepository,
            IEventTypeRepository eventTypeRepository,
            ITimeZoneDataRepository timeZoneDataRepository,
            IUserInfo applicationUser,
            IDateTimeService dateTimeService
            )
        {
            this.availabilityRepository = availabilityRepository;
            this.eventTypeRepository = eventTypeRepository;
            this.timeZoneDataRepository = timeZoneDataRepository;
            this.applicationUser = applicationUser;
            this.dateTimeService = dateTimeService;
        }


        public async Task<Guid> Handle(CreateEventTypeCommand request, CancellationToken cancellationToken)
        {
            var newEventTypeId = Guid.NewGuid();

            var listOfAvailabilities = await availabilityRepository.GetScheduleListByUserId(applicationUser.UserId);

            if (listOfAvailabilities == null || !listOfAvailabilities.Any())
                throw new CustomException("There is no availability configured yet.");

            var defaultAvailability = listOfAvailabilities.Count(e => e.IsDefault) > 0
                ? listOfAvailabilities.First(e => e.IsDefault)
                : listOfAvailabilities.First();

            EventType eventTypeInfo = ConvertToEntity(newEventTypeId, defaultAvailability, request);

            var listOfDefaultQuestions = Util.ApplicationUtil.GetDefaultQuestion();

            eventTypeInfo.Questions = listOfDefaultQuestions;

            await eventTypeRepository.AddNewEventType(eventTypeInfo);

            return await Task.FromResult(newEventTypeId);
        }

        private EventType ConvertToEntity(Guid newId, Availability availability, CreateEventTypeCommand request)
        {
            var listScheduleDetails = CopyScheduleItemFromAvailabilityDetails(newId, availability.Details);

            return new EventType
            {
                Id = newId,
                Name = request.Name,
                OwnerId = applicationUser.UserId,
                Description = request.Descripton,
                EventColor = request.EventColor,
                Slug = request.Slug,
                Location = request.Location,
                ActiveYN = false,
                TimeZoneId = availability.TimeZoneId,
                AvailabilityId = availability.Id,
                DateForwardKind = Constants.Events.ForwandDateKInd.Moving,
                ForwardDuration = Constants.Events.ForwardDuration,
                Duration = Constants.Events.MeetingDuration,
                BufferTimeBefore = Constants.Events.BufferTimeDuration,
                BufferTimeAfter = Constants.Events.BufferTimeDuration,
                CreatedBy = applicationUser.UserId,
                CreatedAt = dateTimeService.GetCurrentTimeUtc,
                EventTypeAvailabilityDetails = listScheduleDetails
            };
        }

        private List<EventTypeAvailabilityDetail> CopyScheduleItemFromAvailabilityDetails(Guid eventTypeId, List<AvailabilityDetail> availabilityDetails)
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

        }
    }
}

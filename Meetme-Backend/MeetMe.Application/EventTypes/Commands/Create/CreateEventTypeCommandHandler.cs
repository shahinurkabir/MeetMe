using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MediatR;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Constant;
using MeetMe.Core.Constants;

namespace MeetMe.Application.EventTypes.Commands.Create
{
    public class CreateEventTypeCommandHandler : IRequestHandler<CreateEventTypeCommand, Guid>
    {
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly ILoginUserInfo applicationUser;
        private readonly IDateTimeService dateTimeService;

        public CreateEventTypeCommandHandler(
            IAvailabilityRepository availabilityRepository,
            IEventTypeRepository eventTypeRepository,
            ILoginUserInfo applicationUser,
            IDateTimeService dateTimeService
            )
        {
            this.availabilityRepository = availabilityRepository;
            this.eventTypeRepository = eventTypeRepository;
            this.applicationUser = applicationUser;
            this.dateTimeService = dateTimeService;
        }


        public async Task<Guid> Handle(CreateEventTypeCommand request, CancellationToken cancellationToken)
        {
            var newEventTypeId = Guid.NewGuid();

            var listOfAvailabilities = await availabilityRepository.GetListByUserId(applicationUser.Id);

            if (listOfAvailabilities == null || !listOfAvailabilities.Any())
                throw new MeetMeException("There is no availability configured yet.");

            var defaultAvailability = listOfAvailabilities.Count(e => e.IsDefault) > 0
                ? listOfAvailabilities.First(e => e.IsDefault)
                : listOfAvailabilities.First();

            EventType eventTypeInfo = ConvertToEntity(newEventTypeId, defaultAvailability, request);

            await eventTypeRepository.AddNewEventType(eventTypeInfo);

            return await Task.FromResult(newEventTypeId);
        }

        private EventType ConvertToEntity(Guid newId, Availability availability, CreateEventTypeCommand request)
        {
            var listScheduleDetails = CopyScheduleItemFromAvailabilityDetails(newId, availability.Details);
            var listQuestions = GetDefaultQuestion();

            return new EventType
            {
                Id = newId,
                Name = request.Name,
                OwnerId = applicationUser.Id,
                Description = request.Description,
                EventColor = request.EventColor,
                Slug = request.Slug,
                Location = request.Location,
                ActiveYN = false,
                TimeZone = availability.TimeZone,
                AvailabilityId = availability.Id,
                DateForwardKind = Events.ForwandDateKInd.Moving,
                ForwardDuration = Events.ForwardDuration,
                Duration = Events.MeetingDuration,
                BufferTimeBefore = Events.BufferTimeDuration,
                BufferTimeAfter = Events.BufferTimeDuration,
                CreatedBy = applicationUser.Id,
                CreatedAt = dateTimeService.GetCurrentTimeUtc,
                EventTypeAvailabilityDetails = listScheduleDetails,
                Questions = listQuestions
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

        private  List<EventTypeQuestion> GetDefaultQuestion()
        {
            var questions = new List<EventTypeQuestion>()
            {
                new EventTypeQuestion
                {
                    Name="Name",
                    QuestionType=Enums.QuestionType.Text.ToString(),
                    ActiveYN=true,
                    DisplayOrder=1 ,
                    RequiredYN=true,
                    SystemDefinedYN=true,
                },
                 new EventTypeQuestion
                {
                    Name="Email",
                    QuestionType=Enums.QuestionType.Text.ToString(),
                    ActiveYN=true,
                    DisplayOrder=2 ,
                    RequiredYN=true,
                    SystemDefinedYN=true,
                }
            };

            return questions;
        }
    }
}

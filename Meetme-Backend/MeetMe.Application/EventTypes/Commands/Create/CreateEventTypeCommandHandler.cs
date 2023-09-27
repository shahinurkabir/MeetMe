using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MediatR;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Constant;
using MeetMe.Core.Constants;
using FluentValidation;

namespace MeetMe.Application.EventTypes.Commands.Create
{
    public class CreateEventTypeCommand : IRequest<Guid>
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string Slug { get; set; } = null!;

        public string EventColor { get; set; } = null!;
        public bool ActiveYN { get; set; }
        public string TimeZoneName { get; set; } = null!;

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(EventColor);
        }


    }

    public class CreateEventTypeCommandHandler : IRequestHandler<CreateEventTypeCommand, Guid>
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IEventTypeRepository _eventTypeRepository;
        private readonly ILoginUserInfo _applicationUser;
        private readonly IDateTimeService _dateTimeService;

        public CreateEventTypeCommandHandler(
            IAvailabilityRepository availabilityRepository,
            IEventTypeRepository eventTypeRepository,
            ILoginUserInfo applicationUser,
            IDateTimeService dateTimeService
            )
        {
            _availabilityRepository = availabilityRepository;
            _eventTypeRepository = eventTypeRepository;
            _applicationUser = applicationUser;
            _dateTimeService = dateTimeService;
        }


        public async Task<Guid> Handle(CreateEventTypeCommand request, CancellationToken cancellationToken)
        {
            var newEventTypeId = Guid.NewGuid();

            var listOfAvailabilities = await _availabilityRepository.GetListByUserId(_applicationUser.Id);

            if (listOfAvailabilities == null || !listOfAvailabilities.Any())
            {
                throw new MeetMeException("There is no availability configured yet.");
            }

            var defaultAvailability = listOfAvailabilities.FirstOrDefault(e => e.IsDefault)?? listOfAvailabilities.First();

            EventType eventTypeInfo = MapCommandToEntity(newEventTypeId, defaultAvailability, request);

            await _eventTypeRepository.AddNewEventType(eventTypeInfo);

            return await Task.FromResult(newEventTypeId);
        }

        private EventType MapCommandToEntity(Guid newId, Availability availability, CreateEventTypeCommand request)
        {
            var listScheduleDetails = MapDefaultScheduleToEntity(newId, availability.Details);

            var listQuestions = GetDefaultQuestion();

            return new EventType
            {
                Id = newId,
                Name = request.Name,
                OwnerId = _applicationUser.Id,
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
                CreatedBy = _applicationUser.Id,
                CreatedAt = _dateTimeService.GetCurrentTimeUtc,
                EventTypeAvailabilityDetails = listScheduleDetails,
                Questions = listQuestions
            };
        }

        private List<EventTypeAvailabilityDetail> MapDefaultScheduleToEntity(Guid eventTypeId, List<AvailabilityDetail> availabilityDetails)
        {
            return  availabilityDetails.Select(e => new EventTypeAvailabilityDetail
            {
                Id = Guid.NewGuid(),
                EventTypeId = eventTypeId,
                DayType = e.DayType,
                Value = e.Value,
                From = e.From,
                To = e.To,
                StepId = e.StepId
            }).ToList();

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

    public class CreateCreateEventTypeCommandValidator : AbstractValidator<CreateEventTypeCommand>
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly ILoginUserInfo applicationUser;

        public CreateCreateEventTypeCommandValidator(IEventTypeRepository eventTypeRepository, ILoginUserInfo applicationUser)
        {
            this.eventTypeRepository = eventTypeRepository;
            this.applicationUser = applicationUser;

            RuleFor(m => m.Name).NotEmpty().WithMessage("Event Type name cannot be empty.");

            RuleFor(m => m.EventColor).NotEmpty().WithMessage("Event Color cannot be empty.");

            RuleFor(m => m.Slug).NotEmpty().WithMessage("Slug cannot be empty.")
                .MustAsync(async (model, slug, token) =>
                {
                    return await CheckNotUsed(model, token);

                }).WithMessage("Slug already used.");

        }

        private async Task<bool> CheckNotUsed(CreateEventTypeCommand command, CancellationToken cancellationToken)
        {
            var listEvents = await eventTypeRepository.GetEventTypeListByUserId(applicationUser.Id);

            var isUsed = listEvents.Count(e =>
            e.Slug.Equals(command.Slug, StringComparison.InvariantCultureIgnoreCase)) > 0;

            return isUsed == false;
        }
    }
}

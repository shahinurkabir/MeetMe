using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Exceptions;

namespace MeetMe.Application.EventTypes.Create
{
    public class CreateEventTypeCommandHandler : IRequestHandler<CreateEventTypeCommand, Guid>
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly ITimeZoneDataRepository timeZoneDataRepository;
        private readonly IUserInfo applicationUser;
        private readonly IDateTimeService dateTimeService;

        public CreateEventTypeCommandHandler(
            IEventTypeRepository eventTypeRepository,
            ITimeZoneDataRepository timeZoneDataRepository,
            IUserInfo applicationUser,
            IDateTimeService dateTimeService
            )
        {
            this.eventTypeRepository = eventTypeRepository;
            this.timeZoneDataRepository = timeZoneDataRepository;
            this.applicationUser = applicationUser;
            this.dateTimeService = dateTimeService;
        }


        public async Task<Guid> Handle(CreateEventTypeCommand request, CancellationToken cancellationToken)
        {
            var newId = Guid.NewGuid();

            EventType eventTypeInfo = ConvertToEntity(request, newId);

            var timeZoneEntity = await timeZoneDataRepository.GetTimeZoneByName(request.TimeZoneName);

            if (timeZoneEntity == null) throw new CustomException($"{request.TimeZoneName} is invalid");

            var timeZoneId = timeZoneEntity.Id;

            var availability = Util.ApplicationUtil.GetDefaultAvailability(newId, timeZoneId);


            var listOfDefaultQuestions = Util.ApplicationUtil.GetDefaultQuestion();

            eventTypeInfo.EventTypeAvailability = availability;
            eventTypeInfo.Questions = listOfDefaultQuestions;

            await eventTypeRepository.AddNewEventType(eventTypeInfo);

            return await Task.FromResult(newId);
        }

        private EventType ConvertToEntity(CreateEventTypeCommand request, Guid newId)
        {
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
                CreatedBy = applicationUser.UserId,
                CreatedAt = dateTimeService.GetCurrentTimeUtc,
            };
        }



    }
}

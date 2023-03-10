using MediatR;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;
using MeetMe.Core.Constant;

namespace MeetMe.Application.Availabilities.Commands.Create
{
    public class CreateAvailabilityCommandHandler : IRequestHandler<CreateAvailabilityCommand, Guid>
    {
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly ITimeZoneDataRepository timeZoneDataRepository;
        private readonly IUserInfo applicationUserInfo;

        public CreateAvailabilityCommandHandler(
            IAvailabilityRepository availabilityRepository, 
            ITimeZoneDataRepository timeZoneDataRepository,
            IUserInfo applicationUserInfo
            )
        {
            this.availabilityRepository = availabilityRepository;
            this.timeZoneDataRepository = timeZoneDataRepository;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CreateAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var userTimeZone=await GetUserTimeZoneData(request);

            var newId = Guid.NewGuid();

            var entity = new Availability
            {
                Id = newId,
                Name = request.Name,
                OwnerId = applicationUserInfo.UserId,
                TimeZoneId = userTimeZone.Id,
                Details=GetDefaultWeeklySchedule(newId)
            };

            _ = await availabilityRepository.AddSchedule(entity);

            return newId;

        }

        private async Task<TimeZoneData> GetUserTimeZoneData(CreateAvailabilityCommand request)
        {
            var offsetInMinutes = request.timeZoneOffset / 60;
            var listTimeZone = await timeZoneDataRepository.GetTimeZoneList();
            var timeZoneData = listTimeZone.FirstOrDefault(e => e.OffsetMinutes == offsetInMinutes);
            if (timeZoneData == null)
                timeZoneData = listTimeZone.First(e => e.Id == 1); // UTC timezone

            return timeZoneData;
        }

        public static List<AvailabilityDetail> GetDefaultWeeklySchedule(Guid availabilityId)
        {
            var dayStartInMinutes = TimeSpan.Parse(Constants.Events.MEETING_FROM_TIMESPAN).TotalMinutes;
            var dayEndINMinutes = TimeSpan.Parse(Constants.Events.MEETING_TO_TIMESPAN).TotalMinutes;

            var listWeekDaysConfig = new List<AvailabilityDetail>();

            short stepId = 0;
            foreach (KeyValuePair<int, string> weekDay in Constants.WeekDays)
            {
                listWeekDaysConfig.Add(new AvailabilityDetail
                {
                    AvailabilityId = availabilityId,
                    DayType = Constants.Events.SCHEDULE_DATETYPE_WEEKDAY,
                    Value = weekDay.Value,
                    From = dayStartInMinutes,
                    To = dayEndINMinutes,
                    StepId = stepId
                });
                stepId++;
            }

            return listWeekDaysConfig;
        }
        
    }
}

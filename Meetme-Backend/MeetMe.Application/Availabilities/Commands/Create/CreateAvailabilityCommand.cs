using MediatR;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;
using MeetMe.Core.Constant;
using MeetMe.Core.Constants;
using FluentValidation;

namespace MeetMe.Application.Availabilities.Commands.Create
{
    public class CreateAvailabilityCommand : IRequest<Guid>
    {
        public string Name { get; set; } = null!;
        public string timeZone { get; set; } = null!;

    }
    public class CreateAvailabilityCommandHandler : IRequestHandler<CreateAvailabilityCommand, Guid>
    {
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly ILoginUserInfo applicationUserInfo;

        public CreateAvailabilityCommandHandler(
            IAvailabilityRepository availabilityRepository, 
            ILoginUserInfo applicationUserInfo
            )
        {
            this.availabilityRepository = availabilityRepository;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CreateAvailabilityCommand request, CancellationToken cancellationToken)
        {

            var newId = Guid.NewGuid();

            var entity = new Availability
            {
                Id = newId,
                Name = request.Name,
                OwnerId = applicationUserInfo.Id,
                TimeZone = request.timeZone,
                IsDefault = false,
                Details=GetDefaultWeeklySchedule(newId)
            };

            _ = await availabilityRepository.AddSchedule(entity);

            return newId;

        }

        public static List<AvailabilityDetail> GetDefaultWeeklySchedule(Guid availabilityId)
        {
            var dayStartInMinutes = TimeSpan.Parse(Events.MEETING_FROM_TIMESPAN).TotalMinutes;
            var dayEndINMinutes = TimeSpan.Parse(Events.MEETING_TO_TIMESPAN).TotalMinutes;

            var listWeekDaysConfig = new List<AvailabilityDetail>();

            short stepId = 0;
            foreach (KeyValuePair<int, string> weekDay in Events.WeekDays)
            {
                listWeekDaysConfig.Add(new AvailabilityDetail
                {
                    AvailabilityId = availabilityId,
                    DayType = Events.SCHEDULE_DATETYPE_WEEKDAY,
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

    public class CreateAvailabilityCommandValidator : AbstractValidator<CreateAvailabilityCommand>
    {
        public CreateAvailabilityCommandValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name can not be empty");
        }

    }
}

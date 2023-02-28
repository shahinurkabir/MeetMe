using MediatR;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;

namespace MeetMe.Application.Availabilities.Commands.Create
{
    public class CreateAvailabilityCommandHandler : IRequestHandler<CreateAvailabilityCommand, Guid>
    {
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly IUserInfo applicationUserInfo;

        public CreateAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository, IUserInfo applicationUserInfo)
        {
            this.availabilityRepository = availabilityRepository;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CreateAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var newId = Guid.NewGuid();

            //var entity = new Availability
            //{
            //    Id = scheduleRuleId,
            //    Name = request.Name,
            //    OwnerId = applicationUserInfo.UserId,
            //    TimeZoneId = request.TimeZoneId
            //};

            //entity.Details = request.RuleItems.ToEntityList(entity.Id);

            var entity = CommandToEntity(request, newId, applicationUserInfo.UserId);
            _ = await availabilityRepository.AddSchedule(entity);


            return newId;

        }

        private static Availability CommandToEntity(CreateAvailabilityCommand command, Guid availabilityId, Guid ownerId)
        {
            var availabilityEntity = new Availability
            {
                Id = availabilityId,
                Name = command.Name,
                OwnerId = ownerId,
                TimeZoneId = command.TimeZoneId
            };

            int itemId = 0;

            command.Details.ForEach(e =>
            {
                var item = new AvailabilityDetail
                {
                    Id = itemId,
                    RuleId = availabilityId,
                    DayType = e.DayType,
                    Value = e.Value,
                    From = e.From,
                    To = e.To
                };

                availabilityEntity.Details.Add(item);

                itemId++;

            });

            return availabilityEntity;

        }
    }
}

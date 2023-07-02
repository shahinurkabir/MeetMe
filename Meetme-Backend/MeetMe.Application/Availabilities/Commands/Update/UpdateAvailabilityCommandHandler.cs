using MediatR;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;

namespace MeetMe.Application.Availabilities.Commands.Update
{
    public class UpdateAvailabilityCommandHandler : IRequestHandler<UpdateAvailabilityCommand, bool>
    {
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly ILoginUserInfo applicationUserInfo;

        public UpdateAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository, ILoginUserInfo applicationUserInfo)
        {
            this.availabilityRepository = availabilityRepository;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<bool> Handle(UpdateAvailabilityCommand request, CancellationToken cancellationToken)
        {

            var entity = await availabilityRepository.GetById(request.Id);

            if (entity == null) throw new Core.Exceptions.MeetMeException("Invalid schedule provided.");

            var listScheduleRuleItems = GetDetailItems(request, entity.Id);

            entity.Name = request.Name;
            entity.TimeZone = request.TimeZone;
            entity.Details = listScheduleRuleItems;

            _ = await availabilityRepository.UpdateSchedule(entity);

            return true;

        }

        private List<AvailabilityDetail> GetDetailItems(UpdateAvailabilityCommand command, Guid availabilityId)
        {

            var listScheduleRuleItems = new List<AvailabilityDetail>();

            int itemId = 0;

            command.Details.ForEach(e =>
            {
                var item = new AvailabilityDetail
                {
                    //Id = itemId,
                    AvailabilityId = availabilityId,
                    DayType = e.DayType,
                    Value = e.Value,
                    From = e.From,
                    To = e.To
                };

                listScheduleRuleItems.Add(item);

                itemId++;

            });

            return listScheduleRuleItems;

        }
    }
}

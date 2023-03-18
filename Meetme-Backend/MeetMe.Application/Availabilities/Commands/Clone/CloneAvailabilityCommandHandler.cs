using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;

namespace MeetMe.Application.Availabilities.Commands.Clone
{
    public class CloneAvailabilityCommandHandler : IRequestHandler<CloneAvailabilityCommand, Guid>
    {
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly IUserInfo applicationUserInfo;

        public CloneAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository, IUserInfo applicationUserInfo)
        {
            this.availabilityRepository = availabilityRepository;
            this.applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CloneAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var newId = Guid.NewGuid();

            var originalModel = await availabilityRepository.GetScheduleById(request.Id);

            var cloneName = $"Clone of {originalModel.Name}";

            var cloneModel = new Availability
            {
                Id = newId,
                Name = cloneName,
                OwnerId = applicationUserInfo.UserId,
                TimeZoneId=originalModel.TimeZoneId,
                Details = originalModel.Details.Select(e =>
                {
                    return new AvailabilityDetail
                    {
                        AvailabilityId = newId,
                        DayType = e.DayType,
                        Value = e.Value,
                        From = e.From,
                        To = e.To,
                        StepId = e.StepId
                    };
                }).ToList(),
            };

            _ = await availabilityRepository.AddSchedule(cloneModel);

            return newId;
        }
    }
}

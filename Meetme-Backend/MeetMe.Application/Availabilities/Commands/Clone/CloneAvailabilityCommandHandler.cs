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
            var newscheduleRule = Guid.NewGuid();

            var originalModel = await availabilityRepository.GetScheduleById(request.AvailabilityId);

            var cloneName = $"Clone of {originalModel.Name}";

            var cloneModel = new Availability
            {
                Id = newscheduleRule,
                Name = cloneName,
                OwnerId = applicationUserInfo.UserId,
                Details = originalModel.Details
            };

            _ = await availabilityRepository.AddSchedule(cloneModel);

            return newscheduleRule;
        }
    }
}

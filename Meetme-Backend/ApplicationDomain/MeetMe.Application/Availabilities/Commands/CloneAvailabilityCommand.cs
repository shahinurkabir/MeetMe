using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;
using MeetMe.Core.Exceptions;

namespace MeetMe.Application.Availabilities.Commands
{
    public class CloneAvailabilityCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

    }

    public class CloneAvailabilityCommandHandler : IRequestHandler<CloneAvailabilityCommand, Guid>
    {
        private readonly IPersistenceProvider persistenceProvider;
        private readonly ILoginUserInfo _applicationUserInfo;

        public CloneAvailabilityCommandHandler(IPersistenceProvider persistenceProvider, ILoginUserInfo applicationUserInfo)
        {
            this.persistenceProvider = persistenceProvider;
            _applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CloneAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var newId = Guid.NewGuid();

            var availabilityEntity = await persistenceProvider.GetAvailability(request.Id);

            if (availabilityEntity == null)
            {
                throw new MeetMeException("Availability not found");

            }

            var cloneName = $"{availabilityEntity.Name} [ Clone ]";

            var cloneModel = new Availability
            {
                Id = newId,
                Name = cloneName,
                OwnerId = _applicationUserInfo.Id,
                TimeZone = availabilityEntity.TimeZone,
                Details = availabilityEntity.Details.Select(e => new AvailabilityDetail
                {
                    AvailabilityId = newId,
                    DayType = e.DayType,
                    Value = e.Value,
                    From = e.From,
                    To = e.To,
                    StepId = e.StepId
                }).ToList()
            };

            await persistenceProvider.AddAvailability(cloneModel);

            return newId;
        }
    }
}

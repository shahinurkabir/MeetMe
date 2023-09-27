using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using MeetMe.Core.Interface;

namespace MeetMe.Application.Availabilities.Commands.Clone
{
    public class CloneAvailabilityCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

    }

    public class CloneAvailabilityCommandHandler : IRequestHandler<CloneAvailabilityCommand, Guid>
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly ILoginUserInfo _applicationUserInfo;

        public CloneAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository, ILoginUserInfo applicationUserInfo)
        {
            _availabilityRepository = availabilityRepository;
            _applicationUserInfo = applicationUserInfo;
        }
        public async Task<Guid> Handle(CloneAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var newId = Guid.NewGuid();

            var originalModel = await _availabilityRepository.GetAvailability(request.Id);

            var cloneName = $"{originalModel.Name} [ Clone ]";

            var cloneModel = new Availability
            {
                Id = newId,
                Name = cloneName,
                OwnerId = _applicationUserInfo.Id,
                TimeZone = originalModel.TimeZone,
                Details = originalModel.Details.Select(e =>
                                     new AvailabilityDetail
                                     {
                                         AvailabilityId = newId,
                                         DayType = e.DayType,
                                         Value = e.Value,
                                         From = e.From,
                                         To = e.To,
                                         StepId = e.StepId
                                     }).ToList()
            };

            await _availabilityRepository.AddAvailability(cloneModel);

            return newId;
        }
    }
}

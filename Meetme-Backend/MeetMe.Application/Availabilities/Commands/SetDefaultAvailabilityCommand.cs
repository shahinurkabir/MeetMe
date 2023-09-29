using MediatR;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Availabilities.Commands
{
    public class SetDefaultAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class SetDefaultAvailabilityCommandHandler : IRequestHandler<SetDefaultAvailabilityCommand, bool>
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly ILoginUserInfo _userInfo;

        public SetDefaultAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository, ILoginUserInfo userInfo)
        {
            _availabilityRepository = availabilityRepository;
            _userInfo = userInfo;
        }
        public async Task<bool> Handle(SetDefaultAvailabilityCommand request, CancellationToken cancellationToken)
        {

            await _availabilityRepository.SetDefaultAvailability(request.Id, _userInfo.Id);

            return true;

        }
    }

}

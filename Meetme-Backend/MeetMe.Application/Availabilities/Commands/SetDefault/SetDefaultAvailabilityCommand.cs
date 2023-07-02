using MediatR;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.Availabilities.Commands.SetDefault
{
    public class SetDefaultAvailabilityCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class SetDefaultAvailabilityCommandHandler : IRequestHandler<SetDefaultAvailabilityCommand, bool>
    {
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly ILoginUserInfo userInfo;

        public SetDefaultAvailabilityCommandHandler(IAvailabilityRepository availabilityRepository, ILoginUserInfo userInfo)
        {
            this.availabilityRepository = availabilityRepository;
            this.userInfo = userInfo;
        }
        public async Task<bool> Handle(SetDefaultAvailabilityCommand request, CancellationToken cancellationToken)
        {
            
            await availabilityRepository.SetDefault(request.Id, userInfo.Id);

            return true;
            
        }
    }

}

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
        private readonly IPersistenceProvider persistenceProvider;
        private readonly ILoginUserInfo _userInfo;

        public SetDefaultAvailabilityCommandHandler(IPersistenceProvider persistenceProvider, ILoginUserInfo userInfo)
        {
            this.persistenceProvider = persistenceProvider;
            _userInfo = userInfo;
        }
        public async Task<bool> Handle(SetDefaultAvailabilityCommand request, CancellationToken cancellationToken)
        {

            await persistenceProvider.SetDefaultAvailability(request.Id, _userInfo.Id);

            return true;

        }
    }

}

using MediatR;
using MeetMe.Application.AccountSettings.Dtos;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.AccountSettings.Queries
{
    public class AccountProfileDetailBySlugQuery : IRequest<AccountProfileDto>
    {
        public string Name { get; set; } = null!;
    }
    public class AccountProfileDetailBySlugQueryHandler : IRequestHandler<AccountProfileDetailBySlugQuery, AccountProfileDto>
    {
        private readonly IPersistenceProvider persistenceProvider;

        public AccountProfileDetailBySlugQueryHandler(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }

        public async Task<AccountProfileDto> Handle(AccountProfileDetailBySlugQuery request, CancellationToken cancellationToken)
        {
            var userEntity = await persistenceProvider.GetUserBySlug(request.Name);

            if (userEntity == null)
            {
                throw new MeetMeException("User not found");
            }

            var userDto = new AccountProfileDto
            {
                Id = userEntity.Id,
                UserName = userEntity.UserName,
                TimeZone = userEntity.TimeZone,
                BaseURI = userEntity.BaseURI,
                WelcomeText = userEntity.WelcomeText
            };

            return userDto;
        }
    }
}



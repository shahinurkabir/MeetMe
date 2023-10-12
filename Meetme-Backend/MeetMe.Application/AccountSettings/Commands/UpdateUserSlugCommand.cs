using FluentValidation;
using MediatR;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.AccountSettings
{
    public class UpdateUserSlugCommand : IRequest<bool>
    {
        public string BaseURI { get; set; } = null!;
    }

    public class UpdateUserSlugCommandHandler : IRequestHandler<UpdateUserSlugCommand, bool>
    {
        private readonly IPersistenceProvider persistenceProvider;
        private readonly ILoginUserInfo _userInfo;

        public UpdateUserSlugCommandHandler(IPersistenceProvider persistenceProvider, ILoginUserInfo userInfo)
        {
            this.persistenceProvider = persistenceProvider;
            _userInfo = userInfo;
        }

        public async Task<bool> Handle(UpdateUserSlugCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await persistenceProvider.GetUserByLoginId(_userInfo.UserId);

            if (userEntity == null)
            {
                throw new MeetMeException("User not found");
            }
            userEntity.BaseURI = request.BaseURI;

            await persistenceProvider.UpdateUser(userEntity);

            return true;

        }
    }

    public class UpdateUserUriCommandValidator : AbstractValidator<UpdateUserSlugCommand>
    {

        public UpdateUserUriCommandValidator(IPersistenceProvider persistenceProvider, ILoginUserInfo userInfo)
        {
            RuleFor(m => m.BaseURI).NotEmpty().WithMessage("Link can not be empty");
            RuleFor(m => m.BaseURI).MustAsync(async (command, slug, token) => (await IsSlugAvailable(persistenceProvider, slug,userInfo.UserId))).WithMessage("Link is already used");
        }

        private async Task<bool> IsSlugAvailable(IPersistenceProvider persistenceProvider,string slug,string userId)
        {
            var userEntity = await persistenceProvider.GetUserBySlug(slug);

            return userEntity==null || userEntity.UserId == userId;

        }

    }
}

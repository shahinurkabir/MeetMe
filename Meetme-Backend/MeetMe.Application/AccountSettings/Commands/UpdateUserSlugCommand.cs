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
        private readonly IUserRepository _userRepository;
        private readonly ILoginUserInfo _userInfo;

        public UpdateUserSlugCommandHandler(IUserRepository userRepository, ILoginUserInfo userInfo)
        {
            _userRepository = userRepository;
            _userInfo = userInfo;
        }

        public async Task<bool> Handle(UpdateUserSlugCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetUserByLoginId(_userInfo.UserId);

            if (userEntity == null)
            {
                throw new MeetMeException("User not found");
            }
            userEntity.BaseURI = request.BaseURI;

            await _userRepository.UpdateUser(userEntity);

            return true;

        }
    }

    public class UpdateUserUriCommandValidator : AbstractValidator<UpdateUserSlugCommand>
    {

        public UpdateUserUriCommandValidator(IUserRepository userRepository, ILoginUserInfo userInfo)
        {
            RuleFor(m => m.BaseURI).NotEmpty().WithMessage("Link can not be empty");
            RuleFor(m => m.BaseURI).MustAsync(async (command, slug, token) => (await IsSlugAvailable(userRepository, slug,userInfo.UserId))).WithMessage("Link is already used");
        }

        private async Task<bool> IsSlugAvailable(IUserRepository userRepository,string slug,string userId)
        {
            var userEntity = await userRepository.GetUserBySlug(slug);

            return userEntity==null || userEntity.UserID == userId;

        }

    }
}

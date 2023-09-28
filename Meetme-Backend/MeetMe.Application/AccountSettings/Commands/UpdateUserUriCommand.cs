using FluentValidation;
using MediatR;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.AccountSettings
{
    public class UpdateUserUriCommand : IRequest<bool>
    {
        public string BaseURI { get; set; } = null!;
    }

    public class UpdateUserUriCommandHandler : IRequestHandler<UpdateUserUriCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginUserInfo _userInfo;

        public UpdateUserUriCommandHandler(IUserRepository userRepository, ILoginUserInfo userInfo)
        {
            _userRepository = userRepository;
            _userInfo = userInfo;
        }

        public async Task<bool> Handle(UpdateUserUriCommand request, CancellationToken cancellationToken)
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

    public class UpdateUserUriCommandValidator : AbstractValidator<UpdateUserUriCommand>
    {

        public UpdateUserUriCommandValidator(IUserRepository userRepository, ILoginUserInfo userInfo)
        {
            RuleFor(m => m.BaseURI).NotEmpty().WithMessage("Link can not be empty");
            RuleFor(m => m.BaseURI).MustAsync(async (command, link, token) => (await IsLinkAvailable(userRepository, link,userInfo.UserId))).WithMessage("Link is already used");
        }

        private async Task<bool> IsLinkAvailable(IUserRepository userRepository,string link,string userId)
        {
            var userEntity = await userRepository.GetUserBySlug(link);

            return userEntity==null || userEntity.UserID == userId;

        }

    }
}

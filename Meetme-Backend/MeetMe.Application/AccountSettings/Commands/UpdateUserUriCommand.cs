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
        private readonly IUserRepository userRepository;
        private readonly ILoginUserInfo userInfo;

        public UpdateUserUriCommandHandler(IUserRepository userRepository, ILoginUserInfo userInfo)
        {
            this.userRepository = userRepository;
            this.userInfo = userInfo;
        }

        public async Task<bool> Handle(UpdateUserUriCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await userRepository.GetByUserId(userInfo.UserId);

            if (userEntity == null)
                throw new MeetMeException("User not found");

            userEntity.BaseURI = request.BaseURI;

            await userRepository.Update(userEntity);

            return await Task.FromResult(true);

        }
    }

    public class UpdateUserUriCommandValidator : AbstractValidator<UpdateUserUriCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly ILoginUserInfo userInfo;

        public UpdateUserUriCommandValidator(IUserRepository userRepository, ILoginUserInfo userInfo)
        {
            RuleFor(m => m.BaseURI).NotEmpty().WithMessage("Link can not be empty");
            RuleFor(m => m.BaseURI).MustAsync(async (command, link, token) => (await userRepository.IsLinkAvailable( link,userInfo.Id))).WithMessage("Link is already used");
            this.userRepository = userRepository;
            this.userInfo = userInfo;
        }

        private async Task<bool> IsUsedByOther(string link)
        {
            var userEntity = await userRepository.GetByBaseURI(link);
            if (userEntity == null)
                return await Task.FromResult(false);

            return userEntity.UserID != userInfo.UserId;

        }

    }
}

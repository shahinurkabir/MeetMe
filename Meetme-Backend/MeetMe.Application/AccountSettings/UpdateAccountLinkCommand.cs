using FluentValidation;
using MediatR;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.AccountSettings
{
    public class UpdateAccountLinkCommand : IRequest<bool>
    {
        public string BaseURI { get; set; } = null!;
    }

    public class UpdateAccountLinkCommandHandler : IRequestHandler<UpdateAccountLinkCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserInfo userInfo;

        public UpdateAccountLinkCommandHandler(IUserRepository userRepository, IUserInfo userInfo)
        {
            this.userRepository = userRepository;
            this.userInfo = userInfo;
        }

        public async Task<bool> Handle(UpdateAccountLinkCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await userRepository.GetByUserId(userInfo.UserId);

            if (userEntity == null)
                throw new MeetMeException("User not found");

            userEntity.BaseURI = request.BaseURI;

            await userRepository.Update(userEntity);

            return await Task.FromResult(true);

        }
    }

    public class UpdateAccountLinkCommandValidator : AbstractValidator<UpdateAccountLinkCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserInfo userInfo;

        public UpdateAccountLinkCommandValidator(IUserRepository userRepository, IUserInfo userInfo)
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

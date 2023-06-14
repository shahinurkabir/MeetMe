using FluentValidation;
using MediatR;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;

namespace MeetMe.Application.AccountSettings
{
    public class SaveAccountLinkCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public string Link { get; set; } = null!;
    }

    public class SaveAccountLinkCommandHandler : IRequestHandler<SaveAccountLinkCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserInfo userInfo;

        public SaveAccountLinkCommandHandler(IUserRepository userRepository, IUserInfo userInfo)
        {
            this.userRepository = userRepository;
            this.userInfo = userInfo;
        }

        public async Task<bool> Handle(SaveAccountLinkCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await userRepository.GetById(userInfo.UserId);

            if (userEntity == null)
                throw new CustomException("User not found");

            userEntity.BaseURI = request.Link;

            await userRepository.Update(userEntity);

            return await Task.FromResult(true);

        }
    }

    public class SaveAccountLinkCommandValidator : AbstractValidator<SaveAccountLinkCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserInfo applicationUserInfo;

        public SaveAccountLinkCommandValidator(IUserRepository userRepository, IUserInfo applicationUserInfo)
        {
            RuleFor(m => m.Link).NotEmpty().WithMessage("Link can not be empty");
            RuleFor(m => m.Link).MustAsync(async (command, link, token) => !(await IsUsedByOther(link))).WithMessage("Link is already used");
            this.userRepository = userRepository;
            this.applicationUserInfo = applicationUserInfo;
        }

        private async Task<bool> IsUsedByOther(string link)
        {
            var userEntity = await userRepository.GetByBaseURI(link);
            if (userEntity == null)
                return await Task.FromResult(false);

            return userEntity.UserID != applicationUserInfo.UserId;

        }

    }
}

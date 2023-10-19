using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.AccountSettings
{
    public class UpdateAccountCommand : IRequest<bool>
    {
        public string UserName { get; set; } = null!;
        public string TimeZone { get; set; } = null!;
        public string? WelcomeText { get; set; }
    }

    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, bool>
    {
        private readonly IPersistenceProvider persistenceProvider;
        private readonly ILoginUserInfo _userInfo;

        public UpdateAccountCommandHandler(IPersistenceProvider persistenceProvider, ILoginUserInfo userInfo)
        {
            this.persistenceProvider = persistenceProvider;
            _userInfo = userInfo;
        }
        public async Task<bool> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await persistenceProvider.GetUserByLoginId(_userInfo.UserId);

            if (userEntity == null)
            {
                throw new MeetMeException("User not found");
            }

            userEntity.UserName = request.UserName;
            userEntity.TimeZone = request.TimeZone;
            userEntity.WelcomeText = request.WelcomeText;

            await persistenceProvider.UpdateUser(userEntity);

            return true;

        }
    }
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            RuleFor(m => m.UserName).NotEmpty().WithMessage("User name can not be empty");
            RuleFor(m => m.TimeZone).NotEmpty().WithMessage("TimeZone can not be empty");
        }
    }

}

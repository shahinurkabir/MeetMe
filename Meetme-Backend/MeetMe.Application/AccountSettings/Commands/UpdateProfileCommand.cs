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
    public class UpdateProfileCommand : IRequest<bool>
    {
        public string UserName { get; set; } = null!;
        public string TimeZone { get; set; } = null!;
        public string? WelcomeText { get; set; }


    }

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginUserInfo _userInfo;

        public UpdateProfileCommandHandler(IUserRepository userRepository, ILoginUserInfo userInfo)
        {
            _userRepository = userRepository;
            _userInfo = userInfo;
        }
        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUserId(_userInfo.UserId);

            if (userEntity == null)
            {
                throw new MeetMeException("User not found");
            }

            userEntity.UserName = request.UserName;
            userEntity.TimeZone = request.TimeZone;
            userEntity.WelcomeText = request.WelcomeText;

            await _userRepository.Update(userEntity);

            return true;

        }
    }
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(m => m.UserName).NotEmpty().WithMessage("User name can not be empty");
            RuleFor(m => m.TimeZone).NotEmpty().WithMessage("TimeZone can not be empty");
        }
    }

}

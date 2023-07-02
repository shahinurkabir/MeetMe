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
    public class UpdateProfileCommand:IRequest<bool>
    {
        public string UserName { get; set; } = null!;
        public string TimeZone { get; set; }=null!;
        public string? WelcomeText { get; set; }


    }

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserInfo userInfo;

        public UpdateProfileCommandHandler(IUserRepository userRepository, IUserInfo userInfo)
        {
            this.userRepository = userRepository;
            this.userInfo = userInfo;
        }
        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await userRepository.GetByUserId(userInfo.UserId);
            if (userEntity == null)
                throw new MeetMeException("User not found");
            userEntity.UserName=request.UserName;
            userEntity.TimeZone = request.TimeZone;
            userEntity.WelcomeText = request.WelcomeText;

            await userRepository.Update(userEntity);
            
            return true;

        }
    }
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand> {
        public UpdateProfileCommandValidator()
        {
            RuleFor(m => m.UserName).NotEmpty().WithMessage("User name can not be empty");
            RuleFor(m => m.TimeZone).NotEmpty().WithMessage("TimeZone can not be empty");
        }
    }

}

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
    public class SaveProfileCommand:IRequest<bool>
    {
        public string Name { get; set; } = null!;
        public int TimeZone { get; set; }
        public string DateFormat { get; set; } = null!;
        public string TimeFormat { get; set; } = null!;
    }

    public class SaveProfileCommandHandler : IRequestHandler<SaveProfileCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IUserInfo userInfo;

        public SaveProfileCommandHandler(IUserRepository userRepository, IUserInfo userInfo)
        {
            this.userRepository = userRepository;
            this.userInfo = userInfo;
        }
        public async Task<bool> Handle(SaveProfileCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await userRepository.GetById(userInfo.UserId);
            if (userEntity == null)
                throw new CustomException("User not found");

            userEntity.TimeZoneId = request.TimeZone;
            userEntity.DateFormat = request.DateFormat;
            userEntity.TimeFormat= request.TimeFormat;

            await userRepository.Update(userEntity);
            
            return true;

        }
    }
    public class SaveProfileCommandValidator : AbstractValidator<SaveProfileCommand> {
        public SaveProfileCommandValidator()
        {
            RuleFor(m => m.TimeZone).NotEmpty().WithMessage("TimeZone can not be empty");
            RuleFor(m => m.DateFormat).NotEmpty().WithMessage("Date Format can not be empty");
            RuleFor(m => m.TimeFormat).NotEmpty().WithMessage("Time Format can not be empty");

        }
    }

}

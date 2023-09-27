using MediatR;
using MeetMe.Application.AccountSettings.Dtos;
using MeetMe.Core.Exceptions;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.Application.AccountSettings.Queries
{

    public class ProfileDetailQuery : IRequest<AccountProfileDto>
    {
        public Guid Id { get; set; }

    }

    public class ProfileDetailQueryHandler : IRequestHandler<ProfileDetailQuery, AccountProfileDto>
    {
        private readonly IUserRepository _userRepository;

        public ProfileDetailQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AccountProfileDto> Handle(ProfileDetailQuery request, CancellationToken cancellationToken)
        {
            var userEntity=await _userRepository.GetById(request.Id);

            if (userEntity==null)
            {
                throw new MeetMeException("User not found");
            }

            var userDto = new AccountProfileDto
            {
                Id = userEntity.Id,
                UserName = userEntity.UserName,
                TimeZone = userEntity.TimeZone,
                BaseURI = userEntity.BaseURI,
                WelcomeText = userEntity.WelcomeText
            };

            return userDto;
        }
    }

    public class ProfileDetailQueryByName : IRequest<AccountProfileDto>
    {
        public string Name { get; set; } = null!;
    }
    public class ProfileDetailsQueryBaseURIHandler : IRequestHandler<ProfileDetailQueryByName, AccountProfileDto>
    {
        private readonly IUserRepository userRepository;

        public ProfileDetailsQueryBaseURIHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<AccountProfileDto> Handle(ProfileDetailQueryByName request, CancellationToken cancellationToken)
        {
            var userEntity = await userRepository.GetByBaseURI(request.Name);

            if (userEntity == null)
            {
                throw new MeetMeException("User not found");
            }

            var userDto = new AccountProfileDto
            {
                Id = userEntity.Id,
                UserName = userEntity.UserName,
                TimeZone = userEntity.TimeZone,
                BaseURI = userEntity.BaseURI,
                WelcomeText = userEntity.WelcomeText
            };

            return userDto;
        }
    }
}



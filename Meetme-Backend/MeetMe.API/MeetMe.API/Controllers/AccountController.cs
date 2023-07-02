using MediatR;
using MeetMe.API.Models;
using MeetMe.Application.AccountSettings;
using MeetMe.Application.AccountSettings.Dtos;
using MeetMe.Application.AccountSettings.Queries;
using MeetMe.Application.Availabilities.Commands.Update;
using MeetMe.Core.Constants;
using MeetMe.Core.Interface;
using MeetMe.Core.Persistence.Entities;
using MeetMe.Core.Persistence.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MeetMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        private readonly ILoginUserInfo userInfo;

        public AccountController(
            IMediator mediator,
            IUserRepository userRepository,
            IConfiguration configuration,
            ILoginUserInfo userInfo
            )
        {
            this.mediator = mediator;
            this.userRepository = userRepository;
            this.configuration = configuration;
            this.userInfo = userInfo;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public async Task<TokenResponse?> GetToken(TokenRequest tokenRequest)
        {
            var userEntity = await userRepository.GetByUserId(tokenRequest.UserId);

            if (userEntity == null || userEntity.Password != tokenRequest.Password) return null;

            var tokenResponse = GenerateToken(userEntity);

            return tokenResponse;

        }

        [HttpGet]
        [Route("profile")]
        public async Task<AccountProfileDto?> GetProfile()
        {
            var response = await mediator.Send(new ProfileDetailQuery { Id = userInfo.Id });

            return response;
        }

        [HttpPost]
        [Route("profile")]
        public async Task<UpdateProfileResponse?> UpdateProfile(UpdateProfileCommand updateProfileCommand)
        {
            var commandResult = await mediator.Send(updateProfileCommand);

            var userEntity = await userRepository.GetById(userInfo.Id);

            var response = new UpdateProfileResponse
            {
                Result = commandResult,
                NewToken = GenerateToken(userEntity!)
            };
            return response;
        }

        [HttpGet]
        [Route("link-available/{link}")]
        public async Task<ActionResult> LinkAvailable(string link)
        {
            var command = new UpdateAccountLinkCommand { BaseURI = link };

            var validator = new UpdateAccountLinkCommandValidator(userRepository, userInfo);
            var result = await validator.ValidateAsync(command);

            if (result.IsValid) { return new OkResult(); }

            return new BadRequestObjectResult(result);
        }

        [HttpPost]
        [Route("link")]
        public async Task<UpdateProfileResponse?> UpdateURI(UpdateAccountLinkCommand updateAccountLinkCommand)
        {
            var commandResult = await mediator.Send(updateAccountLinkCommand);

            var userEntity = await userRepository.GetById(userInfo.Id);

            var response = new UpdateProfileResponse
            {
                Result = commandResult,
                NewToken = GenerateToken(userEntity!)
            };

            return response;
        }


        private TokenResponse GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypeName.Id,user.Id.ToString()),
                new Claim(ClaimTypeName.UserId,user.UserID),
                new Claim(ClaimTypeName.UserName,user.UserName),
                new Claim(ClaimTypeName.BaseURI,user.BaseURI),
                new Claim(ClaimTypeName.TimeZone,user.TimeZone),
            };

            var tokenResponse = new JwtTokenHandler(configuration).CreateToken(claims);

            return tokenResponse;
        }
    }
}

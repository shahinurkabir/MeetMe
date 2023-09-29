using MediatR;
using MeetMe.API.Models;
using MeetMe.Application.AccountSettings;
using MeetMe.Application.AccountSettings.Dtos;
using MeetMe.Application.AccountSettings.Queries;
using MeetMe.Application.Availabilities.Commands;
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
        private readonly IPersistenceProvider persistenceProvider;
        private readonly IConfiguration configuration;
        private readonly ILoginUserInfo userInfo;

        public AccountController(IMediator mediator, IPersistenceProvider persistenceProvider,IConfiguration configuration, ILoginUserInfo userInfo)
        {
            this.mediator = mediator;
            this.persistenceProvider = persistenceProvider;
            this.configuration = configuration;
            this.userInfo = userInfo;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public async Task<TokenResponse?> GetToken(TokenRequest tokenRequest)
        {
            var userEntity = await persistenceProvider.GetUserByLoginId(tokenRequest.UserId);

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
        [HttpGet]
        [Route("profile/{name}")]
        public async Task<AccountProfileDto?> GetProfileByName(string name)
        {
            var response = await mediator.Send(new ProfileDetailQueryByName { Name = name });

            return response;
        }


        [HttpPost]
        [Route("profile")]
        public async Task<UpdateProfileResponse?> UpdateProfile(UpdateProfileCommand updateProfileCommand)
        {
            var commandResult = await mediator.Send(updateProfileCommand);

            var userEntity = await persistenceProvider.GetUserById(userInfo.Id);

            var response = new UpdateProfileResponse
            {
                Result = commandResult,
                NewToken = GenerateToken(userEntity!)
            };
            return response;
        }

        [HttpGet]
        [Route("uri-available/{uri}")]
        public async Task<ActionResult> LinkAvailable(string uri)
        {
            var command = new UpdateUserSlugCommand { BaseURI = uri };

            var validator = new UpdateUserUriCommandValidator(persistenceProvider, userInfo);
            var result = await validator.ValidateAsync(command);

            if (result.IsValid) { return new OkResult(); }

            return new BadRequestObjectResult(result);
        }

        [HttpPost]
        [Route("update-uri")]
        public async Task<UpdateProfileResponse?> UpdateURI(UpdateUserSlugCommand updateAccountLinkCommand)
        {
            var commandResult = await mediator.Send(updateAccountLinkCommand);

            var userEntity = await persistenceProvider.GetUserById(userInfo.Id);

            var response = new UpdateProfileResponse
            {
                Result = commandResult,
                NewToken = GenerateToken(userEntity!)
            };

            return response;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<AccountProfileDto?> GetUserById(Guid id)
        {
            var response = await mediator.Send(new ProfileDetailQuery { Id = id });

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

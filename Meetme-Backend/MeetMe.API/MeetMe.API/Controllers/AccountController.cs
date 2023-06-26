using MeetMe.API.Models;
using MeetMe.Core.Constants;
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
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;

        public AccountController(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public async Task<TokenResponse?> GetToken(TokenRequest tokenRequest)
        {
            var userEntity = await userRepository.GetById(tokenRequest.UserId);

            if (userEntity == null || userEntity.Password != tokenRequest.Password) return null; 

            var tokenResponse = GenerateToken(userEntity);
            
            return tokenResponse;

        }
        private TokenResponse GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypeName.Id,user.Id.ToString()),
                new Claim(ClaimTypeName.UserId,user.UserID),
                new Claim(ClaimTypeName.Email,user.Email),
                new Claim(ClaimTypeName.BaseURI,user.BaseURI),
                new Claim(ClaimTypeName.TimeZoneId,user.TimeZoneId.ToString()),
            };

            var tokenResponse=new JwtTokenHandler(configuration).CreateToken(claims);
            
            return tokenResponse ;
        }
    }
}

using MeetMe.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MeetMe.API
{
    public class JwtTokenHandler
    {
        private readonly IConfiguration configuration;

        public JwtTokenHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public TokenResponse CreateToken(IEnumerable<Claim> claims)
        {
            var tokenConfig = configuration.GetSection("JwtConfig");

            var key = tokenConfig["Secret"];
            var issuer = tokenConfig["Issuer"];
            var audience = tokenConfig["Audience"];
            var expireIn = Convert.ToInt32(tokenConfig["ExpireIn"]);

            var now = DateTime.UtcNow;

            var expireTimeStamp = now.AddMinutes(expireIn);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: now,
                expires: expireTimeStamp,
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            var response = new TokenResponse
            {
                Token = token,
                ExpiredAt = expireTimeStamp.Subtract(now).TotalSeconds
            };

            return response;
        }


    }
}

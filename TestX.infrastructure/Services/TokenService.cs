using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestX.application.Repositories;
using TestX.domain.Entities.AccountRole;

namespace TestX.infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _symmetricSecurityKey;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:secretKey"] ?? string.Empty));
        }
        public string CreateToken(ApplicationUser user)
        {
            string jwtId = Guid.NewGuid().ToString();
            string issuer = _configuration["Jwt:issuer"] ?? string.Empty;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Iss, issuer)
            };
            var cred = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var TokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = user.UserName,
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = cred,
                Issuer = issuer

            };
            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(TokenDesc);
            return tokenhandler.WriteToken(token);
        }
    }
}

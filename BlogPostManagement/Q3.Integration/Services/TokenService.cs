using Microsoft.Azure.Documents;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using Q3.Data.Entities;
using Q3.Shared.DTO.MainData;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ITokenService = Q3.Shared.Interfaces.ITokenService;

namespace Q3.Integration.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(UserDto userDto)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
        new Claim(ClaimTypes.Name, userDto.Username),
        new Claim(ClaimTypes.Role, userDto.Role)


            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}

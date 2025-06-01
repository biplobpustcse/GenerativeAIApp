using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GenerativeAIApp.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GenerativeAIApp.BLL.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly SigningCredentials _creds;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenService(IConfiguration configuration)
        {
            _config = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            _creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: _creds
            );

            return _tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken(string token)
        {
            var claims = new[] { new Claim(ClaimTypes.Name, token) };

            var newToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: _creds
            );
            return _tokenHandler.WriteToken(newToken);
        }
    }
}

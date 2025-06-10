using DapperLearn.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DapperLearn.Helper
{
    public class GenerateJWTToken
    {
        private readonly IConfiguration _configuration;

        public GenerateJWTToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(Users users)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, users.userId.ToString()),
                new Claim(ClaimTypes.Name, users.name),
                new Claim(ClaimTypes.Email, users.email),
                new Claim(ClaimTypes.Role, users.role)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(24),
                claims: claims,
                signingCredentials: credentials
            );

            string tokenGeneration = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenGeneration;
        }
    }
}

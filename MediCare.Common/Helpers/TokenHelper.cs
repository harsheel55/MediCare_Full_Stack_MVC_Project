using MediCare_MVC_Project.MediCare.Application.DTOs;
using MediCare_MVC_Project.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediCare_MVC_Project.MediCare.Common.Helpers
{
    public class JWTTokenHelper
    {
        public readonly IConfiguration _configuration;
        public JWTTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Token generation
        internal string GenerateToken(LoginViewModel login, int roleId, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Dictionary<int, string> roles = new Dictionary<int, string> { { 1, "Administrator" }, { 2, "Doctor" }, { 3, "Receptionist" } };
            string newRole = roles[roleId];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, login.Email),
                new Claim(ClaimTypes.Role, newRole)
            };

            int tokenExpireTime = int.Parse(_configuration["Jwt:ExpireTimeInMinutes"] ?? "10");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpireTime),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
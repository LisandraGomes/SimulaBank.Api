using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimulaBank.Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SimulaBank.Infrastructure.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IConfiguration _configuration;
        public AuthServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string ComputeHash(string password)
        {
            using (var hash = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = hash.ComputeHash(passwordBytes);

                var builder = new StringBuilder(hashBytes.Length * 2);
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string GenerateToken(string userLogin, string role)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            string audience = _configuration["Jwt:Audience"];
            string issuer = _configuration["Jwt:Issuer"];

            var credential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userlogin", userLogin),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credential
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

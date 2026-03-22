using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimulaBank.Domain.Entities;
using SimulaBank.Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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

        public string GenerateToken(string userEmail, string userCpf, bool emailAutorized, string role, string roleDescription, List<Permission> permissions)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            string audience = _configuration["Jwt:Audience"];
            string issuer = _configuration["Jwt:Issuer"];

            var credential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var roleJson = JsonSerializer.Serialize(new { Id = role, Description = roleDescription });
            var user = JsonSerializer.Serialize(new { Cpf = userCpf, Email = userEmail, Authorized = emailAutorized });
            var permission = JsonSerializer.Serialize(permissions.Select(x => x.Nome).ToList());

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, roleDescription),
                new Claim("user", user),
                new Claim("roleJson", roleJson),
                new Claim("permissions", permission)
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

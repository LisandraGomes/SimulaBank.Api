using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SimulaBank.Domain.Interfaces.Services;
using SimulaBank.Infrastructure.Services;
using System.Security.Claims;
using System.Text;

namespace SimulaBank.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUnitOfWork>(provider => new UnitOfWork(configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // Aqui você vê o erro real: se a chave é inválida, se expirou, etc.
                        Console.WriteLine("Falha na autenticação: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        // Se chegar aqui, o token é válido, mas talvez a Role esteja errada (gerando 403)
                        Console.WriteLine("Token validado com sucesso!");
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    RoleClaimType = ClaimTypes.Role,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }
    }
}

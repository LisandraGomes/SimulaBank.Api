using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SimulaBank.Application.Application;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Validators;
using SimulaBank.Data.Repositories;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Interfaces.Services;
using SimulaBank.Domain.Messages;
using SimulaBank.Domain.Utils;
using SimulaBank.Infrastructure.Services;

namespace SimulaBank.Application.DependencyInjection
{
    public static class DependenyInjectionApplication
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddAutoMapper();
            services.AddInfraestructure(configuration);
            services.AddRepositories();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            //services.AddFluentValidationAutoValidation();
            //services.AddValidatorsFromAssemblyContaining<LoginAuthValidators>();
            
            services.AddTransient<LoginAuthValidators>();
            services.AddTransient<VerifyTypeService>();
            services.AddScoped<IAuthApplication, AuthApplication>();
            return services;
        }
        private static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
            });
            return services;
        }
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
        private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            //services.AddAutoMapper(cfg =>
            //{
            //    cfg.AddProfile<>();
            //}, typeof(MapeamentoMotos).Assembly);
            return services;
        }
    }
}

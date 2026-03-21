
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SimulaBank.Application.Application;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Validators;
using SimulaBank.Data.Repositories;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Interfaces.Services;
using SimulaBank.Domain.Utils;

namespace SimulaBank.Application.DependencyInjection
{
    public static class DependenyInjectionApplication
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddAutoMapper();
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
            services.AddScoped<IUserApplication, UserApplication>();
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

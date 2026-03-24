
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
using System.Security;

namespace SimulaBank.Application.DependencyInjection
{
    public static class DependenyInjectionApplication
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthApplication, AuthApplication>();
            services.AddScoped<IUserApplication, UserApplication>();
            services.AddScoped<IBankApplication, BankApplication>();
            services.AddScoped<ITransactionApplication, TransactionApplication>();

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
            return services;
        }
        
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
            services.AddScoped<IPiggyRepository, PiggyRepository>();
            services.AddScoped<IHistoryPiggyRepository, HistoryPiggyRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
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

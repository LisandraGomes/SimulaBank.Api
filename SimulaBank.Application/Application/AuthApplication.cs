using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Outputs;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Interfaces.Services;
using SimulaBank.Domain.Messages;
using SimulaBank.Domain.Utils;
using System.Net;

namespace SimulaBank.Application.Application
{
    public class AuthApplication : IAuthApplication
    {
        private readonly IAuthServices _authServices;
        private readonly IUserRepository _userRepository;
        public AuthApplication(IAuthServices jwtServices,
            IUserRepository userRepository)
        {
            _authServices = jwtServices;
            _userRepository = userRepository;
        }

        public async Task<PatternResult<AuthOutput>> Login(AuthUserInput auth)
        {
            if (string.IsNullOrEmpty(auth.Password))
                return new PatternResult<AuthOutput>(HttpStatusCode.Unauthorized,ResultMessages.PasswordIsNull);

            string cpf = VerifyTypeService.IsCpf(auth.Login) ? auth.Login : string.Empty;
            string email = VerifyTypeService.IsEmail(auth.Login) ? auth.Login : string.Empty;
            string token = string.Empty;

            if (string.IsNullOrEmpty(cpf) && string.IsNullOrEmpty(email))
                return new PatternResult<AuthOutput>(HttpStatusCode.Unauthorized, ResultMessages.UnprocesabledLoginNull);
            
            var user = await _userRepository.GetUserByLogin(email, cpf);
            if (user is null)
                return new PatternResult<AuthOutput>(HttpStatusCode.NotFound,ResultMessages.UserNotFound);

            string passwordHash = _authServices.ComputeHash(auth.Password);
            bool isPasswordValid = await _userRepository.CheckThePassword(user.Cpf, passwordHash);

            if (isPasswordValid)
            {
                token = _authServices.GenerateToken(user.Email, user.IdRole.ToString());
            }

            return new PatternResult<AuthOutput>(new AuthOutput { Token = token });
        }
    }
}

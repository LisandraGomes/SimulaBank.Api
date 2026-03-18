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
        private readonly IAuthServices _jwtServices;
        private readonly IUserRepository _userRepository;
        public AuthApplication(IAuthServices jwtServices,
            IUserRepository userRepository)
        {
            _jwtServices = jwtServices;
            _userRepository = userRepository;
        }
        // Substitua as instâncias de "new PatternResult<AuthOutput>()" por "new PatternResult<AuthOutput>(...)" usando o construtor correto.
        // Supondo que PatternResult<T> tenha um construtor que aceita uma instância de T ou uma mensagem de erro.

        public async Task<PatternResult<AuthOutput>> Login(AuthUserInput auth)
        {
            if (string.IsNullOrEmpty(auth.Password))
                return new PatternResult<AuthOutput>(HttpStatusCode.Unauthorized,ResultMessages.PasswordIsNull);

            string cpf = VerifyTypeService.IsCpf(auth.Login) ? auth.Login : string.Empty;
            string email = VerifyTypeService.IsEmail(auth.Login) ? auth.Login : string.Empty;
            string token = string.Empty;

            if (cpf is null && email is null)
                return new PatternResult<AuthOutput>(HttpStatusCode.Unauthorized, ResultMessages.UnprocesabledLoginNull);
            
            var user = await _userRepository.GetUserByLogin(email, cpf);
            if (user is null)
                return new PatternResult<AuthOutput>(HttpStatusCode.NotFound,ResultMessages.UserNotFound);

            string passwordHash = _jwtServices.ComputeHash(auth.Password);
            bool isPasswordValid = await _userRepository.CheckThePassword(user.Cpf, passwordHash);

            if (isPasswordValid)
            {
                token = _jwtServices.GenerateToken(user.Email, user.IdRole.ToString());
            }

            return new PatternResult<AuthOutput>(new AuthOutput { Token = token });
        }
         
        public async Task<string> Register(RegisterUserInput request)
        {
            var passwordHash = await Task.FromResult(_jwtServices.ComputeHash(request.Password));

            return await Task.FromResult("Register");
        }

    }
}

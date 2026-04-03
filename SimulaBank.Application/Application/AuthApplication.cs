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
        private readonly IUserPermissionRepository _userPermissionRepository;
        public AuthApplication(IAuthServices jwtServices,
            IUserRepository userRepository,
            IUserPermissionRepository userPermissionRepository)
        {
            _authServices = jwtServices;
            _userRepository = userRepository;
            _userPermissionRepository = userPermissionRepository;
        }

        public async Task<PatternResult<AuthOutput>> Login(AuthUserInput auth)
        {
            if (string.IsNullOrEmpty(auth.Password))
                return new PatternResult<AuthOutput>(HttpStatusCode.Unauthorized, ResultMessages.PasswordIsNull);

            string cpf = VerifyTypeService.IsCpf(auth.Login) ? auth.Login : string.Empty;
            string email = VerifyTypeService.IsEmail(auth.Login) ? auth.Login : string.Empty;
            string token = auth.TokenRefreshOrAcess;

            if (string.IsNullOrEmpty(cpf) && string.IsNullOrEmpty(email))
                return new PatternResult<AuthOutput>(HttpStatusCode.Unauthorized, ResultMessages.UnprocesabledLoginNull);

            var user = await _userRepository.GetUserByLogin(email, cpf);
            if (user is null)
                return new PatternResult<AuthOutput>(HttpStatusCode.NotFound, ResultMessages.UserNotFound);

            var permissions = await _userPermissionRepository.GetPermissions(user.Id);

            string passwordHash = _authServices.ComputeHash(auth.Password);
            bool isPasswordValid = await _userRepository.CheckThePassword(user.Cpf, passwordHash);

            if (isPasswordValid)
            {
               token = _authServices.GenerateToken(user.Id,user.Email, user.Cpf, user.EmailAthorization, user.IdRole.ToString(), user.RoleName, permissions);
               return new PatternResult<AuthOutput>(new AuthOutput { Token = token });
            }
            else
                return new PatternResult<AuthOutput>(HttpStatusCode.NotFound, ResultMessages.UserNotFound);

        }
    }
}

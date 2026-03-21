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
    public class UserApplication : IUserApplication
    {
        private readonly IAuthServices _authServices;
        private readonly IUserRepository _userRepository;
        public UserApplication(IAuthServices authServices,
            IUserRepository userRepository)
        {
            _authServices = authServices;
            _userRepository = userRepository;
        }
        public async Task<PatternResult> Register(RegisterUserInput request)
        {
            try
            {
                if (!VerifyTypeService.IsCpf(request.Cpf))
                    return new PatternResult(HttpStatusCode.UnprocessableEntity, ResultMessages.CpfNotCorrect);
                if (!VerifyTypeService.IsEmail(request.Email))
                    return new PatternResult(HttpStatusCode.UnprocessableEntity, ResultMessages.EmailNotCorrect);
                var date = DateTime.Parse(request.BirthDate);
                 if (DateTime.Parse(request.BirthDate) > DateTime.Now.AddYears(-18))
                    return new PatternResult(HttpStatusCode.UnprocessableEntity, ResultMessages.ClientMustBeAdult);

                var user = await _userRepository.GetUserByLogin(request.Email, request.Cpf);
                if (user is not null && user.Id != Guid.Empty)
                    return new PatternResult(HttpStatusCode.OK, ResultMessages.UserExisting);
                else
                {
                    var passwordHash = await Task.FromResult(_authServices.ComputeHash(request.Password));
                    await _userRepository.InsertUser(request.Name, request.MidName, request.Cpf, request.Email, passwordHash, DateTime.Parse(request.BirthDate), (int)request.IdTipoUsuario);
                    var result = new PatternResult(HttpStatusCode.OK, String.Format(ResultMessages.UserRegisterSucess, request.Name));
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }

        public async Task<PatternResult> UpdateUser(Guid id, UpdateUserInput request)
        {
            throw new NotImplementedException();
        }

        public async Task<PatternResult> DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}

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
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountDomainService _accountDomainService;
        public UserApplication(IAuthServices authServices,
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IAccountDomainService accountDomainService)
        {
            _authServices = authServices;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }
        public async Task<PatternResult> Register(RegisterUserInput request)
        {
            try
            {
                var valid = await ValidInputRegister(request);
                if (valid is not null && !string.IsNullOrEmpty(valid.Message))
                    return valid;

                var user = await _userRepository.GetUserByLogin(request.Email, request.Cpf);
                if (user is not null && user.Id != Guid.Empty)
                    return new PatternResult(HttpStatusCode.OK, ResultMessages.UserExisting);
                else
                {
                    var passwordHash = await Task.FromResult(_authServices.ComputeHash(request.Password));
                    var idUser = await _userRepository.InsertUser(request.Name, request.MidName, request.Cpf, request.Email, passwordHash, DateTime.Parse(request.BirthDate), (int)request.IdTipoUsuario);
                    if (idUser == Guid.Empty)
                        return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);

                    var result = new PatternResult(HttpStatusCode.OK, String.Format(ResultMessages.UserRegisterSucess, request.Name));
                    return result;
                }
            }
            catch (Exception ex)
            {
                return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }
        private async Task<PatternResult?> ValidInputRegister(RegisterUserInput input)
        {
            if (!VerifyTypeService.IsCpf(input.Cpf))
                return new PatternResult(HttpStatusCode.UnprocessableEntity, ResultMessages.CpfNotCorrect);
            if (!VerifyTypeService.IsEmail(input.Email))
                return new PatternResult(HttpStatusCode.UnprocessableEntity, ResultMessages.EmailNotCorrect);
            var date = DateTime.Parse(input.BirthDate);
            if (DateTime.Parse(input.BirthDate) > DateTime.Now.AddYears(-18))
                return new PatternResult(HttpStatusCode.UnprocessableEntity, ResultMessages.ClientMustBeAdult);
            else return null;
        }

        public async Task<PatternResult> Active(ActiveUserInput input)
        {
            var mensagem = await ValidInputActiveUser(input);
            if (!string.IsNullOrEmpty(mensagem))
                return new PatternResult(HttpStatusCode.UnprocessableEntity, mensagem);

            bool active = await _userRepository.ActiveUser(input.Token, input.Email);

            if (active)
            {
                var account = await _accountRepository.GetByUserId(input.Token);
                if (account.Id != Guid.Empty)
                {
                    string accountNumber = await _accountDomainService.GenerateNumberAccount();
                    active = await _accountRepository.Create(input.Token, Convert.ToInt32(accountNumber), 0, DateTime.Now, true) != Guid.Empty;
                }
                return new PatternResult(HttpStatusCode.OK, ResultMessages.UpdateSuccess);
            }

            return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
        }
        private async Task<string> ValidInputActiveUser(ActiveUserInput input)
        {
            string mensagem = string.Empty;
            if (string.IsNullOrEmpty(input.Email))
                mensagem = ResultMessages.EmailNotCorrect;
            if (input.Token == Guid.Empty)
                mensagem = ResultMessages.UserNotFound;
            return mensagem;
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

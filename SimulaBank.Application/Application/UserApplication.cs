using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Outputs;
using SimulaBank.Domain.Enum;
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
        private readonly IEmailService _emailService;
        private readonly IPatternEmailRepository _patternEmailRepository;
        private readonly IHistoryEmailsRepository _historyEmailsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserApplication(IAuthServices authServices,
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IAccountDomainService accountDomainService,
            IEmailService emailService,
            IPatternEmailRepository patternEmailRepository,
            IHistoryEmailsRepository historyEmailsRepository,
            IUnitOfWork unitOfWork)
        {
            _authServices = authServices;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _accountDomainService = accountDomainService;
            _emailService = emailService;
            _patternEmailRepository = patternEmailRepository;
            _historyEmailsRepository = historyEmailsRepository;
            _unitOfWork = unitOfWork;
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
                    // 1. Você precisa abrir a conexão ANTES de qualquer query no banco
                    _unitOfWork.BeginTransaction();
                    var passwordHash = await Task.FromResult(_authServices.ComputeHash(request.Password));
                    var idUser = await _userRepository.InsertUser(request.Name, request.MidName, request.Cpf, request.Email, passwordHash, DateTime.Parse(request.BirthDate), (int)request.IdTipoUsuario);
                    if (idUser == Guid.Empty)
                        return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);

                    await ConfirmationEmail(idUser);

                    var result = new PatternResult(HttpStatusCode.OK, String.Format(ResultMessages.UserRegisterSucess, request.Name));
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }
        private async Task ConfirmationEmail(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                var patternEmail = await _patternEmailRepository.GetById((int)EEmailPattern.Confirmation);
                var body = patternEmail.Body.Replace("{0}", user.Name.ToString()).Replace("{1}", user.Id.ToString()).Replace("{2}", user.Email);
                await _emailService.SendEmailAsync(user.Email, patternEmail.Subject, body);

                await _historyEmailsRepository.Create(user.Email, (int)EEmailPattern.Confirmation, true, DateTime.Now);

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw; // Importante relançar ou logar adequadamente
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
            try
            {
                var mensagem = await ValidInputActiveUser(input);
                if (!string.IsNullOrEmpty(mensagem))
                    return new PatternResult(HttpStatusCode.UnprocessableEntity, mensagem);

                bool active = await _userRepository.ActiveUser(input.Token, input.Email);

                if (active)
                {
                    var account = await _accountRepository.GetByUserId(input.Token);
                    if (account is null || account.Id != Guid.Empty)
                    {
                        string accountNumber = await _accountDomainService.GenerateNumberAccount();
                        active = await _accountRepository.Create(input.Token, Convert.ToInt32(accountNumber), 0, DateTime.Now, true) != Guid.Empty;
                    }
                    return new PatternResult(HttpStatusCode.OK, ResultMessages.UpdateSuccess);
                }

                return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
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

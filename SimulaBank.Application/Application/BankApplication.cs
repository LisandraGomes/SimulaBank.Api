using SimulaBank.Application.Input;
using SimulaBank.Application.Interfaces;
using SimulaBank.Application.Outputs;
using SimulaBank.Domain.Enum;
using SimulaBank.Domain.Interfaces.Repositories;
using SimulaBank.Domain.Messages;
using System.Net;

namespace SimulaBank.Application.Application
{
    public class BankApplication : IBankApplication
    {
        private readonly IPiggyRepository _piggyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHistoryPiggyRepository _historyPiggyRepository;
        public BankApplication(IPiggyRepository piggyRepository,
            IUserRepository userRepository,
            IHistoryPiggyRepository historyPiggyRepository)
        {
            _piggyRepository = piggyRepository;
            _userRepository = userRepository;
            _historyPiggyRepository = historyPiggyRepository;
        }

        public async Task<PatternResult> CreatePiggy(PiggyRegisterInput request, string cpf)
        {
            try
            {
                // Validar os dados de entrada com o fluent validation
                var user = await _userRepository.GetUserByLogin(string.Empty, cpf);

                if (user.IdRole != (int)EUserTypes.InvetstorClient)
                    return new PatternResult(HttpStatusCode.Unauthorized, ResultMessages.UserUnauthorized);
                else
                {
                    if (request.MonthOfDeadline > 0)
                        request.DueDate = DateTime.Now.AddMonths((int)request.MonthOfDeadline);

                    bool autoDecduct = request.DayAutoDeductValueAccount > 0 && request.ValueAutoDeductValueAccount > 0;
                    var id = await _piggyRepository.Create(request.Title, request.Description, request.CurrentValue, (int)request.Status,
                        request.CreateDate ?? DateTime.Now, request.DueDate, request.DayAutoDeductValueAccount,
                        request.ValueAutoDeductValueAccount, autoDecduct, true, request.IdUser);
                    
                    await _historyPiggyRepository.Create(id.ToString(), (int)ETypeHistoryPiggy.Create, request.CurrentValue, DateTime.Now);

                    return new PatternResult(HttpStatusCode.OK, ResultMessages.CreateSuccess);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }
        public async Task<PatternResult<List<PiggyOutput>>> GetAllPiggyByUserId(Guid userId, bool onlyActive)
        {
            try
            {
                var piggy = await _piggyRepository.GetAllByUserId(userId, onlyActive);
                if (piggy == null || piggy.Count <= 0)
                    return new PatternResult<List<PiggyOutput>>(HttpStatusCode.NotFound, ResultMessages.PiggyNotFound);

                var output = piggy.Select(x => new PiggyOutput
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    GoalValue = x.GoalValue,
                    CurrentValue = x.CurrentValue,
                    Status = (EStratusPiggyBank)x.Status,
                    CreateDate = x.CreateDate,
                    DueDate = x.DueDate,
                    DayAutoDeductValueAccount = x.DayAutoDeductValueAccount,
                    ValueAutoDeductValueAccount = x.ValueAutoDeductValueAccount,
                    ActiveAutoDeduct = x.ActiveAutoDeduct,
                    Active = x.Active
                }).ToList();
                return new PatternResult<List<PiggyOutput>>(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PatternResult<List<PiggyOutput>>(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }

        public async Task<PatternResult<PiggyOutput>> GetBankBoxById(Guid id)
        {
            try
            {
                var piggy = await _piggyRepository.GetById(id);
                if (piggy == null)
                    return new PatternResult<PiggyOutput>(HttpStatusCode.NotFound, ResultMessages.PiggyNotFound);
                var output = new PiggyOutput
                {
                    Id = piggy.Id,
                    Title = piggy.Title,
                    Description = piggy.Description,
                    GoalValue = piggy.GoalValue,
                    CurrentValue = piggy.CurrentValue,
                    Status = (EStratusPiggyBank)piggy.Status,
                    CreateDate = piggy.CreateDate,
                    DueDate = piggy.DueDate,
                    DayAutoDeductValueAccount = piggy.DayAutoDeductValueAccount,
                    ValueAutoDeductValueAccount = piggy.ValueAutoDeductValueAccount,
                    ActiveAutoDeduct = piggy.ActiveAutoDeduct
                };
                return new PatternResult<PiggyOutput>(output);
            }
            catch (Exception ex)
            {
                return new PatternResult<PiggyOutput>(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }

        public async Task<PatternResult> DeleteBankBox(Guid id)
        {
            try
            {
                await _piggyRepository.Inative(id);
                await _historyPiggyRepository.Create(id.ToString(), (int)ETypeHistoryPiggy.Delete, 0, DateTime.Now);
                return new PatternResult(HttpStatusCode.OK, ResultMessages.DeleteSuccess);
            }
            catch (Exception ex)
            {
                return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }

        public async Task<PatternResult> UpdateBankBox(Guid id)
        {
            try
            {
                // Lógica para atualizar a caixa bancária
                return new PatternResult(HttpStatusCode.OK, ResultMessages.UpdateSuccess);
            }
            catch (Exception ex)
            {
                return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }
    }
}

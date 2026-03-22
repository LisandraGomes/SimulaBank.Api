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
        public BankApplication(IPiggyRepository piggyRepository)
        {
            _piggyRepository = piggyRepository;
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
                    await _piggyRepository.CreatePiggy(request.Title, request.Description, request.CurrentValue, (int)request.Status,
                        request.CreateDate ?? DateTime.Now, request.DueDate, request.DayAutoDeductValueAccount,
                        request.ValueAutoDeductValueAccount, autoDecduct, true, request.IdUser);

                    return new PatternResult(HttpStatusCode.OK, ResultMessages.UserLogonSuccess);
                }
            }
            catch (Exception ex)
            {
                return new PatternResult(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }
        public async Task<PatternResult<List<PiggyOutput>>> GetAllPiggyByUserId(Guid userId)
        {
            try
            {
                var piggy = await _piggyRepository.GetAllPiggyByUserId(userId);
                if (piggy == null)
                {
                    return new PatternResult<List<PiggyOutput>>(HttpStatusCode.NotFound, ResultMessages.PiggyNotFound);
                }
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
                    ActiveAutoDeduct = x.ActiveAutoDeduct
                }).ToList();
                return new PatternResult<List<PiggyOutput>>(output);
            }
            catch (Exception ex)
            {
                return new PatternResult<List<PiggyOutput>>(HttpStatusCode.InternalServerError, ResultMessages.InternalError);
            }
        }
    }
}

using SimulaBank.Application.Input;
using SimulaBank.Application.Outputs;
using SimulaBank.Domain.Messages;
using System.Net;

namespace SimulaBank.Application.Application
{
    public class BankApplication
    {
        public BankApplication()
        {
            
        }

        public async Task<PatternResult> CreateBankBox(PiggyBoxRegisterInput request)
        {

            return new PatternResult(HttpStatusCode.OK, ResultMessages.UserLogonSuccess);
        }
    }
}

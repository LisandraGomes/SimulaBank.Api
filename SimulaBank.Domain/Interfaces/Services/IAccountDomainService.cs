namespace SimulaBank.Domain.Interfaces.Services
{
    public interface IAccountDomainService
    {
        Task<string> GenerateNumberAccount();
    }
}

namespace SimulaBank.Domain.Interfaces.Services
{
    public interface IAuthServices
    {
        string ComputeHash(string password);
        string GenerateToken(string userId, string role);
    }
}

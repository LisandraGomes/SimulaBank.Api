namespace SimulaBank.Domain.Interfaces.Repositories
{
    public interface IHistoryEmailsRepository
    {
        Task Create(string emailUser, int patternEmailId, bool send, DateTime? dateSend);
        Task UpdateAcepted(int id, bool send, DateTime? dateSend);
    }
}

using SimulaBank.Domain.Enum;

namespace SimulaBank.Application.Input
{
    public class TransactionInput
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Value { get; set; }
        public ETypeTransaction TypeId { get; set; }
        public string? IdAccountOrigin { get; set; }
        public string? IdAccountDestination { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateFinally { get; set; }
        public bool Active { get; set; }
    }
}

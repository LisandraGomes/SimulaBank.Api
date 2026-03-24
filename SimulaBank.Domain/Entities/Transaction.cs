namespace SimulaBank.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Value { get; set; }
        public int TypeId { get; set; }
        public string? IdAccountOrigin { get; set; }
        public string? IdAccountDestination { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateFinally { get; set; }
        public bool Active { get; set; }

        // Construtor padrão
        public Transaction()
        {
        }

    }
}

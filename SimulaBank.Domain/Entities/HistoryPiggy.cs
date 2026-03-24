namespace SimulaBank.Domain.Entities
{
    public class HistoryPiggy
    {
        public int Id { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ValueTransaction { get; set; }
        public int PiggyId { get; set; }
        public int TypeHistoryId { get; set; }
        public string UserCreate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}

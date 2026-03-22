namespace SimulaBank.Domain.Entities
{
    public class Piggy
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal GoalValue { get; set; }
        public decimal CurrentValue { get; set; } = 0;
        public int Status { get; set; }
        public Guid UserId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int DayAutoDeductValueAccount { get; set; }
        public decimal ValueAutoDeductValueAccount { get; set; }
        public bool ActiveAutoDeduct { get; set; }
        public bool Active { get; set; }
    }
}

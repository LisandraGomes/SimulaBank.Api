using SimulaBank.Domain.Enum;

namespace SimulaBank.Application.Input
{
    public class PiggyRegisterInput
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal GoalValue { get; set; }
        public decimal? IdealCurrentValue { get; set; }
        public decimal CurrentValue { get; set; }
        public EStratusPiggyBank Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? MonthOfDeadline { get; set; }
        public DateTime DueDate { get; set; }
        public decimal ValueAutoDeductValueAccount { get; set; }
        public int DayAutoDeductValueAccount { get; set; }
        public Guid IdUser { get; set; }
    }
}

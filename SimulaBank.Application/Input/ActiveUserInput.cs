namespace SimulaBank.Application.Input
{
    public class ActiveUserInput
    {
        public Guid Token { get; set; }
        public bool Accepted { get; set; }
        public string Email { get; set; }
    }
}

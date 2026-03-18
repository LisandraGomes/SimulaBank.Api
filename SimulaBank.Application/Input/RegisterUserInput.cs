namespace SimulaBank.Application.Input
{
    public class RegisterUserInput
    {
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string BirthDate { get; set; }
        public string Password { get; set; }
        public int IdTipoUsuario { get; set; }
        public bool UserMaster { get; set; }
    }
}

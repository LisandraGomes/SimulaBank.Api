using SimulaBank.Domain.Enum;

namespace SimulaBank.Application.Input
{
    public class RegisterUserInput
    {
        public string Name { get; set; }
        public string MidName { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string BirthDate { get; set; }
        public string Password { get; set; }
        public EUserTypes IdTipoUsuario { get; set; }
        public bool UserMaster { get; set; }
    }
}

using System.Security;

namespace SimulaBank.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MidName { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        public bool EmailAthorization { get; set; }
        public int IdRole { get; set; }
        public string RoleDescription { get; set; }

        public User()
        {
             
        }

        public User(Guid id, string name, string midName, string cpf, DateTime birthDate, string password, string role, bool active)
        {
            Id = id;
            Name = name;
            MidName = midName;
            Cpf = cpf;
            BirthDate = birthDate;
            Password = password;
            Role = role;
            Active = active;
        }
    }
}

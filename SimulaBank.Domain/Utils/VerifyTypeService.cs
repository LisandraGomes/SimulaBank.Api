namespace SimulaBank.Domain.Utils
{
    public class VerifyTypeService
    {
        public static bool IsCpf(string cpf)
        {
            return cpf.All(char.IsDigit) && cpf.Length == 11;
        }

        public static bool IsEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

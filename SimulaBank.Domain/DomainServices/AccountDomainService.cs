using SimulaBank.Domain.Interfaces.Services;

namespace SimulaBank.Domain.DomainServices
{
    public class AccountDomainService : IAccountDomainService
    {
        private string sequencial;
        public async Task<string> GenerateNumberAccount()
        {
            await GetSequencial();
            string numero = sequencial.ToString().PadLeft(6, '0');
            int soma = 0;
            int peso = 2;

            // Cálculo simplificado do Módulo 11
            for (int i = numero.Length - 1; i >= 0; i--)
            {
                soma += (int)char.GetNumericValue(numero[i]) * peso;
                peso++;
                if (peso > 9) peso = 2;
            }

            int resto = soma % 11;
            int dv = (resto < 2) ? 0 : 11 - resto;

            return $"{numero}{dv}";
        }

        private async Task GetSequencial()
        {
            int valor = System.Random.Shared.Next(0, 100000);
            sequencial = valor.ToString();
            await Task.CompletedTask;
        }

    }
}

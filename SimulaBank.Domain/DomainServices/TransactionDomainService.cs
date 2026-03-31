using SimulaBank.Domain.Enum;
using System.Text.RegularExpressions;

namespace SimulaBank.Domain.DomainServices
{
    public static class TransactionDomainService
    {
        private static readonly TimeSpan MAX_SCHEDULE_AHEAD = TimeSpan.FromDays(30);

        public static (bool IsValid, string ErrorMessage) Validate(decimal value, DateTime dateCreate, DateTime dateFinally, ETypeTransaction typeId, string? idAccountDestination, string? idAccountOrigin)
        {
            if (!HasValidDates(dateCreate, dateFinally, out var dateMsg))
                return (false, dateMsg);

            switch (typeId)
            {
                case ETypeTransaction.Deposit:
                    if (string.IsNullOrWhiteSpace(idAccountDestination))
                        return (false, "Depósito requer conta destino.");
                    if (!IsValidAccountId(idAccountDestination))
                        return (false, "Conta destino em formato inválido.");
                    break;

                case ETypeTransaction.Withdraw:
                    if (string.IsNullOrWhiteSpace(idAccountOrigin))
                        return (false, "Saque requer conta origem.");
                    if (!IsValidAccountId(idAccountOrigin))
                        return (false, "Conta origem em formato inválido.");
                    if (!string.IsNullOrWhiteSpace(idAccountDestination))
                        return (false, "Saque não deve ter conta destino.");
                    break;

                case ETypeTransaction.Transfer:
                    if (string.IsNullOrWhiteSpace(idAccountOrigin) || string.IsNullOrWhiteSpace(idAccountDestination))
                        return (false, "Transferência requer conta origem e destino.");
                    if (!IsValidAccountId(idAccountOrigin) || !IsValidAccountId(idAccountDestination))
                        return (false, "Formato de conta inválido.");
                    if (string.Equals(idAccountOrigin, idAccountDestination, StringComparison.OrdinalIgnoreCase))
                        return (false, "Conta origem e destino não podem ser iguais.");
                    break;

                default:
                    return (false, "Tipo de transação inválido.");
            }

            return (true, null);
        }

        private static bool HasValidDates(DateTime dateCreate, DateTime dateFinally, out string? message)
        {
            if (dateFinally < dateCreate)
            {
                message = "Data de execução (DateFinally) não pode ser anterior à data de criação (DateCreate).";
                return false;
            }

            if (dateFinally > DateTime.UtcNow + MAX_SCHEDULE_AHEAD)
            {
                message = $"Agendamento muito à frente. Máximo permitido: {MAX_SCHEDULE_AHEAD.TotalDays} dias.";
                return false;
            }

            message = null;
            return true;
        }

        private static bool IsValidAccountId(string? accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId)) return false;
            var rx = new Regex(@"^[A-Za-z0-9\-]{5,34}$", RegexOptions.Compiled);
            return rx.IsMatch(accountId);
        }
    }
}

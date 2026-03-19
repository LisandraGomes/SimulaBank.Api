using System.ComponentModel;

namespace SimulaBank.Domain.Enum
{
    public enum EStratusPiggyBank
    {
        [Description("Em Progresso")]
        EmProgresso = 0,
        [Description("Concluído")]
        Concluido = 1,
        [Description("Cancelado")]
        Cancelado = 2,
        [DescriptionAttribute("Pausado")]
        Pausado = 3
    }
}

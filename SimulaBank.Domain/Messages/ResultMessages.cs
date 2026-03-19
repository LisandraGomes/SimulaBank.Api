namespace SimulaBank.Domain.Messages
{
    public static class ResultMessages
    {
        public const string UserNotFound = "Usuario ou senha inválido, por favor verifique os dados.";
        public const string UnprocesabledLoginNull = "Informe os dados corretamente para prosseguir.";
        public const string PasswordIsNull = "Informe a senha para proseguir.";
        public const string CpfNotCorrect = "Cpf não está corretamente formatado.";
        public const string EmailNotCorrect = "Email não está corretamente formatado.";
        public const string ClientMustBeAdult = "Usuário deve ser maior de 18 anos.";
        public const string UserLogonSuccess = "Login realizado com sucesso.";
        public const string UserExisting = "Usuário já cadastrado.";
    }
}
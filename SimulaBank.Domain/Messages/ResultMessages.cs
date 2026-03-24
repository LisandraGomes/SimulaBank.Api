namespace SimulaBank.Domain.Messages
{
    public static class ResultMessages
    {
        public const string UnprocesabledLoginNull = "Informe os dados corretamente para prosseguir.";
        public const string PasswordIsNull = "Informe a senha para proseguir.";
        public const string CpfNotCorrect = "Cpf não está corretamente formatado.";
        public const string EmailNotCorrect = "Email não está corretamente formatado.";
        public const string ClientMustBeAdult = "Usuário deve ser maior de 18 anos.";

        #region User        
        public const string UserNotFound = "Usuario ou senha inválido, por favor verifique os dados.";
        public const string UserLogonSuccess = "Login realizado com sucesso.";
        public const string UserRegisterSucess = "Usuario {0} registrado com sucesso.";
        public const string UserExisting = "Usuário já cadastrado.";
        public const string UserUnauthorized = "Usuário não tem permissão para realizar está atividade.";
        #endregion

        #region Piggy 
        public const string PiggyNotFound = "Usuario não possui cofrinho criado e ativo.";
        #endregion

        #region Pattern
        public const string UpdateSuccess = "Atualização realizada com sucesso.";
        public const string CreateSuccess = "Cadastro realizado com sucesso.";
        public const string DeleteSuccess = "Exclusão realizada com sucesso.";
        public const string InternalError = "Algo deu errado, tente novamente.";
        #endregion

    }
}
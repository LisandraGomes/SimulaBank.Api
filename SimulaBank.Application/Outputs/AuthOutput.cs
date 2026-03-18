namespace SimulaBank.Application.Outputs
{
    public class AuthOutput
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }      
        
        public static AuthOutput Create(string token, string refreshToken)
        {
            return new AuthOutput
            {
                Token = token,
                RefreshToken = refreshToken
            };
        }
    }
}

namespace Models.Dtos.Identity
{
    /// <summary>
    /// Dto для возврата токена после успешной аутентификации
    /// </summary>
    public class AuthTokenDto
    {
        /// <summary>
        /// Токен
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Время жизни
        /// </summary>
        public int Lifetime { get; set; }
    }
}
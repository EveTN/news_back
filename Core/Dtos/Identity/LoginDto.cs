namespace Core.Dtos.Identity
{
    /// <summary>
    /// Dto для аутентификации
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
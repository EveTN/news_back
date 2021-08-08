namespace Core.ConfigureOptions
{
    /// <summary>
    /// Конфигурация для аутентификации
    /// </summary>
    public class AuthTokenOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }

        public int Lifetime { get; set; }
    }
}
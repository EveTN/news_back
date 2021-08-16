using System;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Core.Generators
{
    /// <summary>
    /// Генератор паролей
    /// </summary>
    internal static class PasswordGenerator
    {
        /// <summary>
        /// Генерация пароля
        /// </summary>
        /// <param name="options">конфигурация паролей</param>
        /// <returns>пароль, соответсвующий требованиям</returns>
        internal static string Generate(PasswordOptions options)
        {
            var length = options.RequiredLength;

            var nonAlphanumeric = options.RequireNonAlphanumeric;
            var digit = options.RequireDigit;
            var lowerCase = options.RequireLowercase;
            var upperCase = options.RequireUppercase;

            var password = new StringBuilder();
            var random = new Random();

            while (password.Length < length)
            {
                var c = (char) random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowerCase = false;
                else if (char.IsUpper(c))
                    upperCase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char) random.Next(33, 48));
            if (digit)
                password.Append((char) random.Next(48, 58));
            if (lowerCase)
                password.Append((char) random.Next(97, 123));
            if (upperCase)
                password.Append((char) random.Next(65, 91));

            return password.ToString();
        }
    }
}
using System;
using System.Threading.Tasks;
using Database.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Core.Validators
{
    /// <summary>
    /// Валидатор пароля и email
    /// </summary>
    public class IdentityValidator
    {
        /// <summary>
        /// Валидация email
        /// </summary>
        /// <param name="email">почта</param>
        /// <returns>bool в зависимости от результата валидации</returns>
        public bool EmailValidation(string email)
        {
            return email.Contains('@');
        }

        /// <summary>
        /// Валидация пароля
        /// </summary>
        /// <param name="context">контекст базы данных</param>
        /// <param name="userManager">класс для управления пользователями</param>
        /// <param name="user">пользователь</param>
        /// <param name="password">пароль для валидации</param>
        /// <returns>identityResult с информацией по валидации</returns>
        /// <exception cref="Exception">исключение при несоответствующем заданным параметрам пароле</exception>
        public async Task<IdentityResult> PasswordValidationAsync(HttpContext context, UserManager<User> userManager,
            User user, string password)
        {
            var passwordValidator =
                context.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;

            if (passwordValidator == null)
                throw new Exception("Error retrieving the passwordValidator service");

            return await passwordValidator.ValidateAsync(userManager, user, password);
        }
    }
}
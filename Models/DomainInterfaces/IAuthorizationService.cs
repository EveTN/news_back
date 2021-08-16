using System.Threading.Tasks;
using Database.Entities.Identity;
using Models.Dtos.Identity;

namespace Models.DomainInterfaces
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <returns>Токен</returns>
        public Task<AuthTokenDto> Login(LoginDto payload);

        /// <summary>
        /// Регистрация пользователей
        /// </summary>
        /// <returns>Создает в базе данных пользователя, которому не надо подтверждать свою почту для входа в систему</returns>
        public Task Register(RegisterDto payload);

        /// <summary>
        /// Сброс пароля
        /// </summary>
        public Task<string> ResetPassword(User user);
    }
}
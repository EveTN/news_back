using System.ComponentModel.DataAnnotations;

namespace Models.Dtos.Administrator
{
    /// <summary>
    /// Dto для регистрации пользователя администратором
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Почта
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string Role { get; set; }
    }
}
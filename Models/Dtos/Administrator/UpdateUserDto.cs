using System;

namespace Models.Dtos.Administrator
{
    /// <summary>
    /// Dto для изменения пользователя администратором
    /// </summary>
    public class UpdateUserDto : CreateUserDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
    }
}
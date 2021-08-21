using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Entities.Entities.Identity
{
    /// <summary>
    /// Пользователт
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime? CreatedDt { get; set; }

        /// <summary>
        /// Дата удаления
        /// </summary>
        public DateTime? DeletedDt { get; set; }
        
        /// <summary>
        /// Дата блокировки
        /// </summary>
        public DateTime? BlockedDt { get; set; }
        
        /// <summary>
        /// Признак был ли сменен пароль после регистрации пользователя через администратора
        /// </summary>
        public bool IsChangedPassword { get; set; }
    }
}
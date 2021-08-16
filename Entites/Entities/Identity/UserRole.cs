using System;
using Microsoft.AspNetCore.Identity;

namespace Database.Entities.Identity
{
    /// <summary>
    /// Связт ролей и пользователя
    /// </summary>
    public class UserRole : IdentityUserRole<Guid>
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; set; }
    }
}
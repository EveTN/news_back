using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Database.Entities.Identity
{
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public class Role : IdentityRole<Guid>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
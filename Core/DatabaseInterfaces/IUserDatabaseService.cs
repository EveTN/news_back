using System;
using System.Threading.Tasks;
using Entities.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.DatabaseInterfaces
{
    public interface IUserDatabaseService
    {
        PasswordOptions GetPasswordOptions();
        Task<bool> CheckForRoleExistenceAsync(string roles);
        Task<IdentityResult> CreateUserAsync(User user, string password, string role = null);
        Task<IdentityResult> UpdateUserAsync(User user, string role);
        Task<User> GetUserAsync(Guid id);
        Task DeleteUserAsync(Guid id);
        Task RestoreUserAsync(Guid id);
        Task BlockUserAsync(Guid id);
        Task UnBlockUserAsync(Guid id);
    }
}
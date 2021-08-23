using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DatabaseInterfaces;
using Entities.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Database.DatabaseServices
{
    public class UserDatabaseService : IUserDatabaseService
    {
        private readonly ILogger<UserDatabaseService> _logger;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserDatabaseService(RoleManager<IdentityRole<Guid>> roleManager, UserManager<User> userManager,
            ILogger<UserDatabaseService> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public PasswordOptions GetPasswordOptions() => _userManager.Options.Password;

        public async Task<bool> CheckForRoleExistenceAsync(string role)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            return roleExists;
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password, string role = null)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                _logger.LogError("The errors occurred while creating user", result.Errors);
                return result;
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            _logger.LogInformation($"User with id: {user.Id} and role: {role} was created");
            return result;
        }

        public async Task<IdentityResult> UpdateUserAsync(User user, string role)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("The errors occurred while updating user", result.Errors);
                return result;
            }

            var currentRole = (await _userManager.GetRolesAsync(user)).SingleOrDefault();

            if (currentRole != null && !currentRole.Equals(role))
            {
                await _userManager.RemoveFromRoleAsync(user, currentRole);
                await _userManager.AddToRoleAsync(user, role);
            }

            if (currentRole == default)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return default;
        }

        public async Task<User> GetUserAsync(Guid id) => await _userManager.FindByIdAsync(id.ToString());

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await GetUserAsync(id);
            user.DeletedDt = DateTime.UtcNow;
            await UpdateWithoutRoleAsync(user);
        }

        public async Task RestoreUserAsync(Guid id)
        {
            var user = await GetUserAsync(id);
            user.DeletedDt = null;
            await UpdateWithoutRoleAsync(user);
        }

        public async Task BlockUserAsync(Guid id)
        {
            var user = await GetUserAsync(id);
            user.BlockedDt = DateTime.UtcNow;
            await UpdateWithoutRoleAsync(user);
        }

        public async Task UnBlockUserAsync(Guid id)
        {
            var user = await GetUserAsync(id);
            user.BlockedDt = null;
            await UpdateWithoutRoleAsync(user);
        }

        private async Task UpdateWithoutRoleAsync(User user) => await _userManager.UpdateAsync(user);
    }
}
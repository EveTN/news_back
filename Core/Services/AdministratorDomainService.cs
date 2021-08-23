using System;
using System.Threading.Tasks;
using Core.DatabaseInterfaces;
using Core.Exception;
using Core.Generators;
using Core.Validators;
using Entities.Constants;
using Entities.Entities.Identity;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Models.DomainInterfaces;
using Models.Dtos.Administrator;

namespace Core.Services
{
    public class AdministratorDomainService : IAdministratorDomainService
    {
        private readonly ILogger<AdministratorDomainService> _logger;
        private readonly IdentityValidator _identityValidator;
        private readonly IUserDatabaseService _userDatabaseService;
        private readonly IMapper _mapper;

        public AdministratorDomainService(IdentityValidator identityValidator, IUserDatabaseService userDatabaseService,
            ILogger<AdministratorDomainService> logger, IMapper mapper)
        {
            _identityValidator = identityValidator;
            _userDatabaseService = userDatabaseService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateUserAsync(CreateUserDto payload)
        {
            await UserValidateAsync(payload.Email, payload.Role);

            var options = _userDatabaseService.GetPasswordOptions();
            var password = PasswordGenerator.Generate(options);

            var user = payload.Adapt<User>(_mapper.Config);
            var result = await _userDatabaseService.CreateUserAsync(user, password, payload.Role);
            if (!result.Succeeded)
            {
                throw new AppException("The errors occurred while creating user");
            }
        }

        public async Task UpdateUserAsync(UpdateUserDto payload)
        {
            await UserValidateAsync(payload.Email, payload.Role);

            var user = await _userDatabaseService.GetUserAsync(payload.Id);
            if (user == default)
            {
                throw new AppException("User not found");
            }

            var updatedUser = _mapper.Map(payload, user);

            var result = await _userDatabaseService.UpdateUserAsync(updatedUser, payload.Role);
            if (result != default && !result.Succeeded)
            {
                _logger.LogInformation($"The errors occurred while updating user with id: {payload.Id}", result.Errors);
                throw new AppException("The errors occurred while update user");
            }

            _logger.LogInformation($"User with id: {payload.Id} was updated");
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userDatabaseService.GetUserAsync(id);
            if (user?.DeletedDt != default)
            {
                _logger.LogInformation($"User with id: {id} doesn't exist for delete");
                throw new AppException("User not exists");
            }

            await _userDatabaseService.DeleteUserAsync(id);
            _logger.LogInformation($"User with id: {id} was deleted");
        }

        public async Task RestoreUserAsync(Guid id)
        {
            var user = await _userDatabaseService.GetUserAsync(id);
            if (user?.DeletedDt == null)
            {
                _logger.LogInformation($"User with id: {id} doesn't exist for restore");
                throw new AppException("User not exists");
            }

            await _userDatabaseService.RestoreUserAsync(id);
            _logger.LogInformation($"User with id: {id} was restored");
        }

        public async Task BlockUserAsync(Guid id)
        {
            var user = await _userDatabaseService.GetUserAsync(id);
            if (user?.DeletedDt != null || user?.BlockedDt != default)
            {
                _logger.LogInformation($"User with id: {id} doesn't exist for block");
                throw new AppException("User not exists");
            }

            await _userDatabaseService.BlockUserAsync(id);
            _logger.LogInformation($"User with id: {id} was blocked");
        }

        public async Task UnBlockUserAsync(Guid id)
        {
            var user = await _userDatabaseService.GetUserAsync(id);
            if (user?.DeletedDt != null || user?.BlockedDt == null)
            {
                _logger.LogInformation($"User with id: {id} doesn't exist for unblock");
                throw new AppException("User not exists");
            }

            await _userDatabaseService.UnBlockUserAsync(id);
            _logger.LogInformation($"User with id: {id} was unblocked");
        }

        private async Task UserValidateAsync(string email, string role)
        {
            if (!_identityValidator.EmailValidation(email))
            {
                _logger.LogInformation("Email is invalid while create/update user");
                throw new System.Exception("Email is invalid");
            }

            if (role.Equals(UserRoleConstants.Administrator) ||
                !await _userDatabaseService.CheckForRoleExistenceAsync(role))
            {
                _logger.LogInformation($"Role {role} doesn't exist");
                throw new System.Exception("Role is invalid");
            }
        }
    }
}
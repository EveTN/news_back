using System;
using System.Threading.Tasks;
using Models.Dtos.Administrator;

namespace Models.DomainInterfaces
{
    /// <summary>
    /// Сервис для администратора
    /// </summary>
    public interface IAdministratorDomainService
    {
        Task CreateUserAsync(CreateUserDto payload);
        Task UpdateUserAsync(UpdateUserDto payload);
        Task DeleteUserAsync(Guid id);
        Task RestoreUserAsync(Guid id);
        Task BlockUserAsync(Guid id);
        Task UnBlockUserAsync(Guid id);
    }
}
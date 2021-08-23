using System;
using System.Threading.Tasks;
using Entities.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DomainInterfaces;
using Models.Dtos.Administrator;

namespace Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    [Authorize]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorDomainService _administratorDomainService;
        private readonly UserManager<User> _userManager;

        public AdministratorController(IAdministratorDomainService administratorDomainService,
            UserManager<User> userManager)
        {
            _administratorDomainService = administratorDomainService;
            _userManager = userManager;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> CreateUser(CreateUserDto payload)
        {
            await _administratorDomainService.CreateUserAsync(payload);
            return Ok();
        }

        [HttpPut("update-user")]
        public async Task UpdateUser(UpdateUserDto payload)
        {
            await _administratorDomainService.UpdateUserAsync(payload);
        }

        [HttpPatch("delete-user")]
        public async Task DeleteUser(Guid id)
        {
            await _administratorDomainService.DeleteUserAsync(id);
        }

        [HttpPatch("restore-user")]
        public async Task RestoreUser(Guid id)
        {
            await _administratorDomainService.RestoreUserAsync(id);
        }

        [HttpPatch("block-user")]
        public async Task BlockUser(Guid id)
        {
            await _administratorDomainService.BlockUserAsync(id);
        }

        [HttpPatch("unblock-user")]
        public async Task UnBlockUser(Guid id)
        {
            await _administratorDomainService.UnBlockUserAsync(id);
        }
    }
}
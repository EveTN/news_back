using System.Threading.Tasks;
using Core.Validators;
using Entities.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DomainInterfaces;
using Models.Dtos.Identity;

namespace Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IdentityValidator _identityValidator;
        private readonly UserManager<User> _userManager;

        public AuthController(ILogger<AuthController> logger, IAuthorizationService authorizationService,
            IdentityValidator identityValidator, UserManager<User> userManager)
        {
            _logger = logger;
            _authorizationService = authorizationService;
            _identityValidator = identityValidator;
            _userManager = userManager;
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto payload)
        {
            if (payload == null)
            {
                return BadRequest("Invalid client request");
            }

            var tokenDto = await _authorizationService.Login(payload);
            return Ok(tokenDto);
        }

        /// <summary>
        /// Регистрация пользователей
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto payload)
        {
            if (!_identityValidator.EmailValidation(payload.Email))
                return BadRequest("Invalid email");

            await _authorizationService.Register(payload);

            return Ok();
        }

        /// <summary>
        /// Сброс пароля
        /// </summary>
        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.EmailConfirmed)
                return NotFound("This user not found");

            var password = await _authorizationService.ResetPassword(user);
            return Ok(password);
        }
    }
}
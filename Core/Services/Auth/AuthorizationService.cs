using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.DatabaseInterfaces;
using Core.Generators;
using Core.Validators;
using Entities;
using Entities.Entities.Identity;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.ConfigureOptions;
using Models.DomainInterfaces;
using Models.Dtos.Identity;

namespace Core.Services.Auth
{
    internal class AuthorizationService : IAuthorizationService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IOptions<AuthTokenOptions> _authOptions;
        private readonly IdentityValidator _validator;
        private readonly ILogger<AuthorizationService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserDatabaseService _userDatabaseService;

        public AuthorizationService(SignInManager<User> signInManager, UserManager<User> userManager,
            ApplicationDbContext context, IOptions<AuthTokenOptions> authOptions, IdentityValidator validator,
            ILogger<AuthorizationService> logger, IMapper mapper, IUserDatabaseService userDatabaseService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _authOptions = authOptions;
            _validator = validator;
            _logger = logger;
            _mapper = mapper;
            _userDatabaseService = userDatabaseService;
        }

        /// <inheritdoc />
        public async Task<AuthTokenDto> Login(LoginDto payload)
        {
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                _logger.LogError("Incorrect login or password", payload);
                throw new ApplicationException("Incorrect login or password");
            }

            if (user.LockoutEnabled)
            {
                _logger.LogError("User account is lockout", payload);
                throw new ApplicationException("You do not have access to your account");
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogError("User account not confirmed", payload);
                throw new ApplicationException("You do not have access to your account");
            }

            var result = await _signInManager.PasswordSignInAsync(user, payload.Password, false, false);
            if (!result.Succeeded)
            {
                _logger.LogError("Incorrect login or password", payload);
                throw new ApplicationException("Incorrect login or password");
            }

            // user.UserRoles = await _context.UserRoles.Where(x => x.UserId == user.Id).ToListAsync();
            return GenerateToken(user);
        }

        /// <inheritdoc />
        public async Task Register(RegisterDto payload)
        {
            if (!_validator.EmailValidation(payload.Email))
            {
                _logger.LogError("Incorrect email", payload);
                throw new ApplicationException("Incorrect email");
            }

            var user = _mapper.Map<User>(payload);

            var result = await _userDatabaseService.CreateUserAsync(user, payload.Password);
            if (result != default && !result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                _logger.LogError("Errors occurred during registration", errors);
                throw new ApplicationException("Errors occurred during registration");
            }
            
            _logger.LogInformation("User with was created");
        }

        /// <inheritdoc />
        public async Task<string> ResetPassword(User user)
        {
            var options = _userManager.Options.Password;
            var password = PasswordGenerator.Generate(options);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User reset his own password", user.Email);
                return password;
            }

            return default;
        }

        /// <summary>
        /// Сгенерировать токен
        /// </summary>
        /// <returns>Токен с его временем жизни</returns>
        private AuthTokenDto GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
            };

            /*if (user.UserRoles != null)
            {
                /*foreach (var item in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }#1#
            }*/

            return new AuthTokenDto
            {
                AccessToken = TokenGenerator.Generate(claims, _authOptions),
                Lifetime = _authOptions.Value.Lifetime
            };
        }
    }
}
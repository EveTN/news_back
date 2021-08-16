using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Generators;
using Core.Validators;
using Database;
using Database.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public AuthorizationService(SignInManager<User> signInManager, UserManager<User> userManager,
            ApplicationDbContext context, IOptions<AuthTokenOptions> authOptions, IdentityValidator validator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _authOptions = authOptions;
            _validator = validator;
        }

        /// <inheritdoc />
        public async Task<AuthTokenDto> Login(LoginDto payload)
        {
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                // logger
                // return BadRequest("Incorrect login or password");
                return default;
            }

            if (user.LockoutEnabled)
            {
                // logger
                // return StatusCode(403, "Your account is lockout");
                return default;
            }

            if (!user.EmailConfirmed)
            {
                // logger
                // return StatusCode(403, "You should confirm your email to get the access");
                return default;
            }

            var result = await _signInManager.PasswordSignInAsync(user, payload.Password, false, false);
            if (!result.Succeeded)
            {
                // logger
                // return BadRequest("Incorrect login or password");
                return default;
            }

            user.UserRoles = await _context.UserRoles.Where(x => x.UserId == user.Id).ToListAsync();
            return GenerateToken(user);
        }

        /// <inheritdoc />
        public async Task Register(RegisterDto payload)
        {
            if (!_validator.EmailValidation(payload.Email))
            {
                // logger
                // return BadRequest("Invalid email");
            }

            var user = new User
            {
                Email = payload.Email,
                EmailConfirmed = true,
                UserName = payload.Email,
                PhoneNumber = payload.PhoneNumber,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                CreatedDt = DateTime.Now,
                FirstName = payload.FirstName,
                LastName = payload.LastName,
                MiddleName = payload.MiddleName
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var result = await _userManager.CreateAsync(user, payload.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description);
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }

            // logger
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
                // logger
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

            if (user.UserRoles != null)
            {
                foreach (var item in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item.Role.Name));
                }
            }

            return new AuthTokenDto
            {
                AccessToken = TokenGenerator.Generate(claims, _authOptions),
                Lifetime = _authOptions.Value.Lifetime
            };
        }
    }
}
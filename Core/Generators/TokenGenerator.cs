using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.ConfigureOptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Core.Generators
{
    /// <summary>
    /// Генератор токенов
    /// </summary>
    public static class TokenGenerator
    {
        /// <summary>
        /// Генерация токена
        /// </summary>
        /// <param name="claims">данные пользователя, для которого генерируется токен</param>
        /// <param name="options">конфигурация для генерации токена</param>
        /// <returns>валидный токен</returns>
        public static string Generate(List<Claim> claims, IOptions<AuthTokenOptions> options)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(6000),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Playlist_for_party.Exceptions.UserExceptions;
using Playlist_for_party.Interfaсes.Services.Managers.UserManagers;
using WebApp_Data.Models.DTO;
using WebApp_Data.Models.UserData;

namespace Playlist_for_party.Services.Managers.UserManagers
{
    public class AuthManager : IAuthManager
    {
        private readonly IEnumerable<char> _unacceptableChars = new List<char>()
        {
            '!', '@', '#', '$', '%', '^', ':', ':', '\\', '/', '_', '=',
            '+', '-', '№', '.', ',', '[', ']', '{', '}',
            '&', '*'
        };

        private const int ExpiresDays = 3;

        public void SetToken(User user, HttpContext context, IConfiguration configuration)
        {
            var token = CreateToken(user, configuration);

            context.Session.Set("Authorization", Encoding.UTF8.GetBytes(token));
        }

        public void ValidateSingUpData(SingUpUserDto singUpUserDto)
        {
            if (singUpUserDto.UserName.Length < 4)
            {
                throw new InvalidUserNameLengthException("UserName lenght is invalid. Must be greater than 3");
            }

            if (_unacceptableChars.Any(unacceptableChar => singUpUserDto.UserName.Contains(unacceptableChar)))
            {
                throw new InvalidSingUpUserNameException("UserName contains invalid symbols");
            }

            if (singUpUserDto.Password.Length < 7)
            {
                throw new InvalidPasswordLengthException("Password lenght is invalid. Must be greater than 6");
            }

            if (singUpUserDto.Password != singUpUserDto.ReEnterPassword)
            {
                throw new PasswordConfirmationException("Please confirm your password");
            }
        }

        private static string CreateToken(User user, IConfiguration configuration)
        {
            var roles = new List<string>() { "user" };
            var token = GenerateToken(configuration, user, roles);
            return token;
        }

        private static string GenerateToken(IConfiguration configuration, User user, IEnumerable<string> roles)
        {
            var jwt = GetJwtSecurityToken(configuration, user, roles);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static JwtSecurityToken GetJwtSecurityToken(IConfiguration configuration, User user,
            IEnumerable<string> roles)
        {
            var jwtClaims = GetClaims(user, roles);
            var singingKey = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(configuration["JWTSettings:SecretKey"] ?? string.Empty));
            var credentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(ExpiresDays);

            var jwt = new JwtSecurityToken(
                issuer: configuration["JWTSettings:Issuer"],
                audience: configuration["JWTSettings:Audience"],
                claims: jwtClaims,
                expires: expires,
                signingCredentials: credentials
            );

            return jwt;
        }

        private static IEnumerable<Claim> GetClaims(User user, IEnumerable<string> roles)
        {
            var jwtClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            roles.ToList()
                .ForEach(role =>
                    jwtClaims.Add(new Claim(ClaimTypes.Role, role)));

            return jwtClaims;
        }
    }
}
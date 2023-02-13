using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Playlist_for_party.Exceptions.UserExceptions;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Models;

namespace Playlist_for_party.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IEnumerable<char> _unacceptableChars = new List<char>()
        {
            '!', '@', '#', '$', '%', '^', ':', ':', '\\', '/', '_', '=', '+', '-', '№', '.', ',', '[', ']', '{', '}',
            '&','*'
        };

        public void SetToken(UserDtoLogin userDto, HttpContext context, IUserManagerService userManager,
            IConfiguration configuration)
        {
            var token = userManager.CreateToken(userDto, configuration);

            context.Session.SetString("Authorization", token);
        }

        public void ValidateSingUpData(UserDtoSingUp userDtoLogin)
        {
            if (userDtoLogin.UserName.Length < 4)
            {
                throw new InvalidUserNameLengthException("UserName lenght is invalid. Must be greater than 3");
            }

            if (_unacceptableChars.Any(unacceptableChar => userDtoLogin.UserName.Contains(unacceptableChar)))
            {
                throw new InvalidSingUpUserNameException("UserName contains invalid symbols");
            }

            if (userDtoLogin.Password.Length < 7)
            {
                throw new InvalidPasswordLengthException("Password lenght is invalid. Must be greater than 6");
            }
        }
    }
}
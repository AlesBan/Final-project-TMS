using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private readonly IUserManagerService _userManager;

        public AuthManager(IUserManagerService userManager)
        {
            _userManager = userManager;
        }
        public void SetToken(User user, HttpContext context, IConfiguration configuration)
        {
            var token = _userManager.CreateToken(user, configuration);

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
    }
}
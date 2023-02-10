using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Models;

namespace Playlist_for_party.Services
{
    public class AuthManager : IAuthManager
    {
        public void SetToken(UserDtoLogin userDto, HttpContext context, IUserManagerService userManager, IConfiguration configuration)
        {
            var token = userManager.CreateToken(userDto, configuration);
                
            context.Session.SetString("Authorization", token);
        }
    }
}
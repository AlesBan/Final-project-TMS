using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp_Data.Models;

namespace Playlist_for_party.Interfaсes.Services
{
    public interface IAuthManager
    {
        void SetToken(UserDtoLogin userDto, HttpContext context, IUserManagerService userManager, IConfiguration configuration);
        void ValidateSingUpData(UserDtoSingUp userDtoLogin);
    }
}
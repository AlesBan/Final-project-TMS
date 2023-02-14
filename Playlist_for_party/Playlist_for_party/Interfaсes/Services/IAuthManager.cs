using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp_Data.Models;

namespace Playlist_for_party.Interfaсes.Services
{
    public interface IAuthManager
    {
        void SetToken(UserDto userDto, HttpContext context, IConfiguration configuration);
        void ValidateSingUpData(UserDto userDtoLogin);
    }
}
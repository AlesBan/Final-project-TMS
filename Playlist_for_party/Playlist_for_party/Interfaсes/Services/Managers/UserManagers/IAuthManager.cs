using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WebApp_Data.Models.DTO;
using WebApp_Data.Models.UserData;

namespace Playlist_for_party.Interfaсes.Services.Managers.UserManagers
{
    public interface IAuthManager
    {
        void SetToken(User user, HttpContext context, IConfiguration configuration);
        void ValidateSingUpData(SingUpUserDto singUpUserDto);
    }
}
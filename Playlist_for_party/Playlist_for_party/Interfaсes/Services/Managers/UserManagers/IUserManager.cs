using Microsoft.AspNetCore.Http;
using WebApp_Data.Models.UserData;

namespace Playlist_for_party.Interfaсes.Services.Managers.UserManagers
{
    public interface IUserManager
    {
        User GetCurrentUser(HttpContext context);
    }
}
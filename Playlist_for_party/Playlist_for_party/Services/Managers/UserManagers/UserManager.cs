using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Playlist_for_party.Exceptions.AppExceptions;
using Playlist_for_party.Interfaсes.Services.Managers.DataManagers;
using Playlist_for_party.Interfaсes.Services.Managers.UserManagers;
using WebApp_Data.Models.UserData;

namespace Playlist_for_party.Services.Managers.UserManagers
{
    public class UserManager : IUserManager
    {
        private readonly IDataManager _dataManager;

        public UserManager(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public User GetCurrentUser(HttpContext context)
        {
            var userName = context
                .User
                .Claims
                .SingleOrDefault(i => i.Type == ClaimTypes.Name)?
                .Value;

            if (string.IsNullOrEmpty(userName))
            {
                throw new InvalidClaimedUserIdException();
            }

            return _dataManager.GetUserByUserName(userName);
        }
    }
}
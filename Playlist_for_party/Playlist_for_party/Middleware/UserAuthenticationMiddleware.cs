using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Playlist_for_party.Data;
using Playlist_for_party.Models;

namespace Playlist_for_party.Middleware
{
    public class UserAuthenticationMiddleware : IMiddleware
    {
        private readonly IDataProtectionProvider _protectionProvider;

        public UserAuthenticationMiddleware(IDataProtectionProvider protectionProvider)
        {
            _protectionProvider = protectionProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Query != null && context.Request.Query.TryGetValue("encryptKey", out var encryptKey))
            {
                var protector = _protectionProvider.CreateProtector("user-auth");
                var decryptedKey = protector.Unprotect(encryptKey);
                var user = JsonConvert.DeserializeObject<User>(decryptedKey);
                var actualUser = Startup.MusicRepository.Users.SingleOrDefault(u => u.UserName == user.UserName);
                context.Items.Add("user-auth", actualUser);
            }

            await next(context);
        }
    }
}
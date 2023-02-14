using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Interfaсes.Services
{
    public interface IUserManagerService
    {
        string CreateToken(UserDto userDto, IConfiguration configuration);
        User GetCurrentUser(HttpContext context);
        void SetRedactor(User user, Playlist playlist);
        void CreatePlaylist(User user, out Playlist playlist);

        void GetUserPlaylistAndTrack(HttpContext context, string trackId, string playlistId,
            out User user, out Playlist playlist, out Track track);

        string GetResultOfAddingAbility(Guid key, User user, Playlist playlist, Track track);
    }
}
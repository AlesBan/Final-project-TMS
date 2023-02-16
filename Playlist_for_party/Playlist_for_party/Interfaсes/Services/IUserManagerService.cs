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
        User GetCurrentUser(HttpContext context);
        void SetRedactor(User user, Playlist playlist);
        Playlist CreatePlaylist(User user);

        void GetPlaylistAndTrack(HttpContext context, string trackId, string playlistId,
            out Playlist playlist, out Track track);

        string GetResultOfAddingAbility(User user, Playlist playlist, Track track);
    }
}
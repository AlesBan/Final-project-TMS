using System;
using System.Collections.Generic;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Interfaсes.Services
{
    public interface IMusicDataManagerService
    {
        void AddUser(User user);
        Playlist CreatePlaylist();
        Playlist GetPlaylist(Guid playlistId);
        IEnumerable<Playlist> GetPlaylists();
        void AddTrack(User user, Playlist playlist, Track track);
        User GetUser(Guid userId);
        User GetUser(string userName);

    }
}
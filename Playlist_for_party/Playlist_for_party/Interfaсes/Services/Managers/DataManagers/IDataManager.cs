using System;
using System.Collections.Generic;
using WebApp_Data.Models.DbConnections;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.UserData;

namespace Playlist_for_party.Interfaсes.Services.Managers.DataManagers
{
    public interface IDataManager
    {
        void CreateUser(User user, List<Role> roles);
        Playlist CreatePlaylist(User user);
        User GetUserByUserName(string userName);
        IEnumerable<User> GetUsers();
        Track GetTrackById(string trackId);
        Playlist GetPlaylistById(Guid playlistId);
        UserEditorPlaylist GetUserEditorPlaylistByPlaylistId(Guid playlistId);
        IEnumerable<Playlist> GetUserOwnerPlaylists(User user);
        IEnumerable<UserEditorPlaylist> GetUserEditorPlaylists(User user);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Playlist_for_party.Data;
using Playlist_for_party.Interfaсes.Services.Managers.DataManagers;
using WebApp_Data.Models.DbConnections;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.UserData;

namespace Playlist_for_party.Services.Managers.DataManagers
{
    public class DataManager : IDataManager
    {
        private readonly MusicContext _musicContext;

        public DataManager(MusicContext musicContext)
        {
            _musicContext = musicContext;
        }

        public void CreateUser(User user, List<Role> roles)
        {
            roles.ForEach(r => _musicContext.Users.SingleOrDefault(u => u.UserName == user.UserName)!.UserRoles.Add(
                new UserRole()
                {
                    User = user,
                    Role = r
                })
            );
            _musicContext.Users.Add(user);
            _musicContext.SaveChangesAsync();
        }

        public Playlist CreatePlaylist(User user)
        {
            var playlist = new Playlist();
            _musicContext.Playlists.Add(playlist);
            AddUserOwnerPlaylists(user, playlist);
            _musicContext.SaveChangesAsync();
            return playlist;
        }

        public User GetUserByUserName(string userName)
        {
            var user = _musicContext.Users.FirstOrDefault(p => p.UserName.Equals(userName));
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            return _musicContext.Users;
        }

        public Track GetTrackById(string trackId)
        {
            return _musicContext.Tracks.SingleOrDefault(t => t.Id == trackId);
        }

        public Playlist GetPlaylistById(Guid playlistId)
        {
            var playlist = _musicContext.Playlists.FirstOrDefault(p => p.Id.Equals(playlistId));
            return playlist;
        }

        public UserEditorPlaylist GetUserEditorPlaylistByPlaylistId(Guid playlistId)
        {
            var userPlaylist = _musicContext.UserEditorPlaylists.Single(up => up.PlaylistId == playlistId);
            return userPlaylist;
        }

        public IEnumerable<Playlist> GetUserOwnerPlaylists(User user)
        {
            return _musicContext.Users.Single(u => u.Equals(user)).UserOwnerPlaylists;
        }

        public IEnumerable<UserEditorPlaylist> GetUserEditorPlaylists(User user)
        {
            return _musicContext.UserEditorPlaylists.Where(x => x.UserId == user.Id);
        }

        private void AddUserOwnerPlaylists(User user, Playlist playlist)
        {
            var userDb = _musicContext.Users.Single(u => u.Equals(user));
            userDb.UserOwnerPlaylists.Add(playlist);
            _musicContext.SaveChangesAsync();
        }
    }
}
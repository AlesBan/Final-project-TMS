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
        private MusicContext MusicContext { get; set; }

        public DataManager(MusicContext musicContext)
        {
            MusicContext = musicContext;
        }

        public void CreateUser(User user)
        {
            MusicContext.Users.Add(user);
            MusicContext.SaveChanges();
        }

        public Playlist CreatePlaylist(User user)
        {
            var playlist = new Playlist();
            MusicContext.Playlists.Add(playlist);
            AddOwnerAndEditorToPlaylists(user, playlist);
            MusicContext.SaveChanges();
            return playlist;
        }

        public User GetUserByUserName(string userName)
        {
            var user = MusicContext.Users
                .FirstOrDefault(p => p.UserName.Equals(userName));
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            return MusicContext.Users;
        }

        public Track GetTrackById(string trackId)
        {
            return MusicContext.Tracks
                .SingleOrDefault(t => t.Id == trackId);
        }

        public Playlist GetPlaylistById(Guid playlistId)
        {
            var playlist = MusicContext.Playlists
                .FirstOrDefault(p => p.Id.Equals(playlistId));
            return playlist;
        }

        public UserEditorPlaylist GetUserEditorPlaylistByPlaylistId(Guid playlistId)
        {
            var userPlaylist = MusicContext.UserEditorPlaylists.Single(up => up.PlaylistId == playlistId);
            return userPlaylist;
        }

        public IEnumerable<Playlist> GetPlaylistsWhereUserOwner(User user)
        {
            return MusicContext.Users
                .Single(u => u.Equals(user))
                .UserOwnerPlaylists;
        }

        public IEnumerable<Playlist> GetPlaylistsWhereUserEditor(User user)
        {
            return MusicContext.UserEditorPlaylists
                .Where(x => x.UserId == user.Id)
                .Select(x => x.Playlist);
        }

        private void AddOwnerAndEditorToPlaylists(User user, Playlist playlist)
        {
            var userDb = MusicContext.Users
                .Single(u => u.Equals(user));
            userDb.UserEditorPlaylists
                .Add(new UserEditorPlaylist()
                {
                    User = user,
                    Playlist = playlist
                });
            userDb.UserOwnerPlaylists
                .Add(playlist);
        }
    }
}
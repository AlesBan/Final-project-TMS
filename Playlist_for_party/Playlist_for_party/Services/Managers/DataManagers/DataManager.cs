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
            var playlist = new Playlist()
            {
                OwnerId = user.Id
            };
            
            MusicContext.Playlists.Add(playlist);
            MusicContext.SaveChanges();
            SetOwnerToPlaylist(user, playlist);
            return playlist;
        }

        public User GetUserByUserName(string userName)
        {
            var user = MusicContext.Users
                .FirstOrDefault(p => p.UserName == userName);
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            try
            {
                return MusicContext.Users;
            }
            catch
            {
                return null;
            }
        }

        public Track GetTrackById(string trackId)
        {
            return MusicContext.Tracks
                .SingleOrDefault(t => t.Id == trackId);
        }

        public int GetTrackRating(string trackId)
        {
            try
            {
                return MusicContext.Tracks
                    .SingleOrDefault(t => t.Id == trackId)!.Rating;
            }
            catch
            {
                return 0;
            }
        }

        public Playlist GetPlaylistById(Guid playlistId)
        {
            var playlist = MusicContext.Playlists
                .FirstOrDefault(p => p.Id == playlistId);
            return playlist;
        }

        public UserEditorPlaylist GetUserEditorPlaylistByPlaylistId(Guid playlistId)
        {
            var userPlaylist = MusicContext
                .UserEditorPlaylists
                .Single(up => up.PlaylistId == playlistId);
            return userPlaylist;
        }

        public IEnumerable<Playlist> GetPlaylistsWhereUserOwner(User user)
        {
            return MusicContext.Playlists
                .Where(u => u.OwnerId == user.Id);
        }

        public IEnumerable<Playlist> GetPlaylistsWhereUserEditor(User user)
        {
            return MusicContext.UserEditorPlaylists
                .Where(x => x.UserId == user.Id)
                .Select(x => x.Playlist);
        }

        private void SetOwnerToPlaylist(User user, Playlist playlist)
        {
            var playlistDb = MusicContext.Playlists
                .SingleOrDefault(u => u.Id == playlist.Id);
            playlistDb!.Owner = user;
            MusicContext.SaveChanges();
        }
    }
}
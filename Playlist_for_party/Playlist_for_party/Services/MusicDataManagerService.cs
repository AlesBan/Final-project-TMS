using System;
using System.Collections.Generic;
using System.Linq;
using Playlist_for_party.Controllers;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Interfaces;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Services
{
    public class MusicDataManagerService : IMusicDataManagerService
    {
        public void AddUser(User user)
        {
            AccountController.MusicRepository.Users.Add(user);
        }
        
        public Playlist GetPlaylist(Guid playlistId)
        {
            var playlist = AccountController.MusicRepository.Playlists.FirstOrDefault(p => p.PlaylistId.Equals(playlistId));
            return playlist;
        }

        public IEnumerable<Playlist> GetPlaylists()
        {
            return AccountController.MusicRepository.Playlists;
        }

        public void AddTrack(User user, Playlist playlist, Track track)
        {
            var playlists = AccountController.MusicRepository.Playlists;
            AccountController.MusicRepository.Playlists[playlists.IndexOf(playlist)]
                .AddTrack(user, track);
        }



        public Playlist CreatePlaylist()
        {
            var playlist = new Playlist();
            AccountController.MusicRepository.Playlists.Add(playlist);
            return playlist;
        }

        public User GetUser(Guid id)
        {
            var user = AccountController.MusicRepository.Users.FirstOrDefault(p => p.UserId.Equals(id));
            return user;
        }

        public User GetUser(UserDto userDto)
        {
            var user = AccountController.MusicRepository.Users.FirstOrDefault(p =>
                p.UserName.Equals(userDto.UserName) && p.Password.Equals(userDto.Password));
            return user;
        }
    }
}
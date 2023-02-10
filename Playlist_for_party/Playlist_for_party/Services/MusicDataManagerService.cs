using System;
using System.Collections.Generic;
using System.Linq;
using Playlist_for_party.Controllers;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Interfaces;
using WebApp_Data.Models;
using WebApp_Data.Models.Data;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Services
{
    public class MusicDataManagerService : IMusicDataManagerService
    {
        public IMusicRepository MusicRepository { get; set; }

        public MusicDataManagerService()
        {
            MusicRepository = new MusicRepository()
            {
                Users = new List<User>()
                {
                    new User()
                    {
                        UserId = Guid.Parse("596fcae8-7491-4940-b39c-8e86c2561dea"),
                        UserName = "ales",
                        Password = "ales"
                    },
                    new User()
                    {
                        UserId = Guid.Parse("e24f63bc-a8eb-4fe3-a7d6-5844c1b30ab4"),
                        UserName = "pavel",
                        Password = "pavel"
                    },
                    new User()
                    {
                        UserId = Guid.Parse("1afeeb58-2f69-422e-842d-0759a7b6825d"),
                        UserName = "dima",
                        Password = "dima"
                    }
                }
            };
        }

        public Playlist GetPlaylist(Guid playlistId)
        {
            var playlist = MusicRepository.Playlists.FirstOrDefault(p => p.PlaylistId.Equals(playlistId));
            return playlist;
        }

        public IEnumerable<Playlist> GetPlaylists()
        {
            return MusicRepository.Playlists;
        }

        public void AddTrack(User user, Playlist playlist, Track track)
        {
            var playlists = MusicRepository.Playlists;
            MusicRepository.Playlists[playlists.IndexOf(playlist)]
                .AddTrack(user, track);
        }

        public Playlist CreatePlaylist()
        {
            var playlist = new Playlist();
            MusicRepository.Playlists.Add(playlist);
            return playlist;
        }

        public User GetUser(Guid id)
        {
            var user = MusicRepository.Users.FirstOrDefault(p => p.UserId.Equals(id));
            return user;
        }

        public User GetUser(UserDtoLogin userDto)
        {
            var user = MusicRepository.Users.FirstOrDefault(p =>
                p.UserName.Equals(userDto.UserName) && p.Password.Equals(userDto.Password));
            return user;
        }
    }
}
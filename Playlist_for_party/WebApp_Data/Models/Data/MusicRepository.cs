using System;
using System.Collections.Generic;
using System.Linq;
using WebApp_Data.Interfaces;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Models.Data
{
    public class MusicRepository : IMusicRepository
    {
        public List<User> Users { get; set; }

        public List<Playlist> Playlists { get; set; } = new List<Playlist>();

        public Playlist CreatePlaylist()
        {
            var playlist = new Playlist();
            Playlists.Add(playlist);
            return playlist;
        }

        public Playlist GetPlaylist(Guid id)
        {
            var playlist = Playlists.FirstOrDefault(p => p.PlaylistId.Equals(id));
            return playlist;
        }

        public User GetUser(Guid id)
        {
            var user = Users.FirstOrDefault(p => p.UserId.Equals(id));
            return user;
        }

        public User GetUser(UserDto userDto)
        {
            var user = Users.FirstOrDefault(p =>
                p.UserName.Equals(userDto.UserName) && p.Password.Equals(userDto.Password));
            return user;
        }
    }
}
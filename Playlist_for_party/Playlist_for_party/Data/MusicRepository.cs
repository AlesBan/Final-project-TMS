using System;
using System.Collections.Generic;
using System.Linq;
using Playlist_for_party.Interfaсes;
using Playlist_for_party.Models;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Data
{
    public class MusicRepository : IMusicRepository
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Playlist> Playlists { get; set; }

        public MusicRepository()
        {
            Playlists = new List<Playlist>();
        }

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
        
    }
}
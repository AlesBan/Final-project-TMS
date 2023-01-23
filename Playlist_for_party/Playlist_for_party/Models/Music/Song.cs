using System;
using System.Collections.Generic;
using Playlist_for_party.Models.Connections;

namespace Playlist_for_party.Models.Music
{
    public class Song
    {
        public Guid SongId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Popularity { get; set; }
        public double Duration { get; set; }
        public string ImageRef { get; set; }
        public ICollection<PlaylistSongs> PlaylistSongs { get; set; }

        
    }
}
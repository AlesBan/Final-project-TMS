using System;
using System.Collections.Generic;

namespace Playlist_for_party.Models.Music
{
    public class Album
    {
        public Guid AlbumId { get; set; }
        public string Title { get; set; }
        public ArtistMy Artist { get; set; }
        public string ImageRef { get; set; }
        public double Duration { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
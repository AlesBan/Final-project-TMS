using System;
using System.Collections.Generic;

namespace Playlist_for_party.Models.Music
{
    public class ArtistMy
    {
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string ImageRef { get; set; }
        public string Href { get; set; }
        
        public ICollection<Album> Albums { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
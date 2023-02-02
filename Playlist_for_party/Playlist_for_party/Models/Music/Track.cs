using System;
using System.Collections.Generic;
using Playlist_for_party.Models.DbConnections;

namespace Playlist_for_party.Models.Music
{
    public class Track
    {
        public string TrackId { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Popularity { get; set; }
        public double Duration { get; set; }
        public string ImageUrl { get; set; }
        public string Href { get; set; }
        public ICollection<PlaylistTracks> PlaylistTracks { get; set; }

        public Track()
        {
            Popularity = 0;
        }
    }
}
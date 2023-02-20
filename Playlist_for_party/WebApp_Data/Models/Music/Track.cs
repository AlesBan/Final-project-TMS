using System;
using System.Collections.Generic;
using WebApp_Data.Models.DbConnections;

namespace WebApp_Data.Models.Music
{
    public class Track
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ArtistName { get; set; }
        public string Album { get; set; }
        public int Rating { get; set; }

        public double DurationMs { get; set; }

        public string Duration
        {
            get
            {
                var t = TimeSpan.FromMilliseconds(DurationMs);
                return $"{t.Minutes:D1}.{t.Seconds:D2}";
            }
        }

        public string ImageUrl { get; set; }
        public string Href { get; set; }
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();

        public Track()
        {
            Rating = 0;
        }
    }
}
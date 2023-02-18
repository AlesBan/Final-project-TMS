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
        public int Popularity { get; set; }
        public double Duration { get; set; }
        public string ImageUrl { get; set; }
        public string Href { get; set; }
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; }

        public Track()
        {
            Popularity = 0;
        }
    }
}
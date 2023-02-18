using System;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Models.DbConnections
{
    public class PlaylistTrack
    {
        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
        public string TrackId { get; set; }
        public Track Track { get; set; }
    }
}
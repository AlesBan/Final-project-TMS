using System;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Models.DbConnections
{
    public class PlaylistTracks
    {
        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
        public Guid TrackId { get; set; }
        public Track Track { get; set; }
    }
}
using System;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Models.Connections
{
    public class PlaylistTracks
    {
        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
        public Guid TrackId { get; set; }
        public Track Track { get; set; }
    }
}
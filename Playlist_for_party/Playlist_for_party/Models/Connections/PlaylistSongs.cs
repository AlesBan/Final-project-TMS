using System;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Models.Connections
{
    public class PlaylistSongs
    {
        public Guid PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
        public Guid SongId { get; set; }
        public Song Song { get; set; }

    }
}
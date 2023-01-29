using System;
using System.Collections.Generic;
using System.Linq;
using Playlist_for_party.Models.Connections;

namespace Playlist_for_party.Models.Music
{
    public class Playlist
    {
        public Guid PlaylistId { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
        public string UrlRef { get; set; }

        public int RedactorsCount
        {
            set => UserEditorPlaylists.Count();
        }

        public string ImageRef { get; set; }

        public double Duration { get; set; }
        public List<Track> Tracks { get; set; }

        public ICollection<PlaylistTracks> PlaylistTracks { get; set; }
        public ICollection<UserEditorPlaylists> UserEditorPlaylists { get; set; }

        public Playlist()
        {
            Tracks = new List<Track>();
        }
        public void AddTrack(Track track)
        {
            Tracks.Add(track);
        }
    }
}
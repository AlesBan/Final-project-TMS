using System;
using System.Collections.Generic;
using System.Linq;
using Playlist_for_party.Models.DbConnections;

namespace Playlist_for_party.Models.Music
{
    public class Playlist
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private static Random random = new Random();
             
        public Guid PlaylistId { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
        public string Href { get; set; }

        public int RedactorsCount
        {
            set => UserEditorPlaylists.Count();
        }

        public string ImageUrl { get; set; }

        public double Duration { get; set; }
        public List<Track> Tracks { get; set; }

        public ICollection<PlaylistTracks> PlaylistTracks { get; set; }
        public ICollection<UserEditorPlaylists> UserEditorPlaylists { get; set; }

        public Playlist()
        {
            Tracks = new List<Track>();
            PlaylistId = Guid.NewGuid();
            Name = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void AddTrack(Track track)
        {
            Tracks.Add(track);
        }
    }
}
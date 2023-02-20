using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using WebApp_Data.Models.DbConnections;
using WebApp_Data.Models.UserData;

namespace WebApp_Data.Models.Music
{
    public class Playlist
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private static readonly Random Random = new Random();

        public Guid Id { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
        public Guid OwnerId { get; set; }
        public string Href { get; set; }

        public string ImageUrl { get; set; }

        [NotMapped]
        public Dictionary<string, int> TracksRating { get; set; }

        public string TracksRatingJson { get; set; }

        [NotMapped]
        public Dictionary<Guid, IEnumerable<Track>> UserTracks { get; set; }
        public string UserTracksJson { get; set; }
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
        public ICollection<UserEditorPlaylist> UserEditorPlaylists { get; set; } = new List<UserEditorPlaylist>();

        public Playlist()
        {
            Id = Guid.NewGuid();
            Name = new string(Enumerable.Repeat(Chars, 8).Select(s => s[Random.Next(s.Length)]).ToArray());
            UserTracks = new Dictionary<Guid, IEnumerable<Track>>();
        }
    }
}
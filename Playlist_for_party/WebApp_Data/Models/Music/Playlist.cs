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

        public double Duration
        {
            get { return PlaylistTracks.Select(pt => pt.Track).Sum(track => track.Duration); }
        }
        
        [NotMapped]
        public Dictionary<Guid, IEnumerable<Track>> UserTracks
        {
            get
            {
                try
                {
                    return JsonSerializer.Deserialize<Dictionary<Guid, IEnumerable<Track>>>(UserTracksJson);
                }
                catch
                {
                    return new Dictionary<Guid, IEnumerable<Track>>();
                }
            }
            private set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                UserTracksJson = JsonSerializer.Serialize(UserTracks);
            }
        }

        public string UserTracksJson { get; set; } = string.Empty;
        public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();

        public ICollection<UserEditorPlaylist> UserEditorPlaylists { get; set; }

        public Playlist()
        {
            Id = Guid.NewGuid();
            Name = new string(Enumerable.Repeat(Chars, 8).Select(s => s[Random.Next(s.Length)]).ToArray());
            UserTracks = new Dictionary<Guid, IEnumerable<Track>>();
        }

        public void AddTrackToUserTracks(User user, Track track)
        {
            var userTracksList = GetUserTracks(user);

            userTracksList.Add(track);
            UserTracks[user.Id] = userTracksList;
        }

        private List<Track> GetUserTracks(User user)
        {
            var userTracks = new List<Track>();
            if (UserTracks.ContainsKey(user.Id))
            {
                userTracks = UserTracks[user.Id].ToList();
                return userTracks;
            }

            UserTracks.Add(user.Id, new List<Track>());
            return userTracks;
        }
    }
}
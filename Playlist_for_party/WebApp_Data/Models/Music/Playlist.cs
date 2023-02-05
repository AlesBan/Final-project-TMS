using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp_Data.Models.Music
{
    public class Playlist
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private static readonly Random Random = new Random();

        public Guid PlaylistId { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
        public string Href { get; set; }

        public int RedactorsCount => Redactors.Count();

        public string ImageUrl { get; set; }

        public double Duration
        {
            get { return Tracks.Sum(track => track.Duration); }
        }

        public Dictionary<Guid, IEnumerable<Track>> UserTracks;
        public List<Track> Tracks { get; set; }
        public List<User> Redactors { get; set; }

        public Playlist()
        {
            Tracks = new List<Track>();
            PlaylistId = Guid.NewGuid();
            Redactors = new List<User>();
            Name = new string(Enumerable.Repeat(Chars, 8).Select(s => s[Random.Next(s.Length)]).ToArray());
            UserTracks = new Dictionary<Guid, IEnumerable<Track>>();
        }

        public void AddTrack(User user, Track track)
        {
            AddTrackToUserTracks(user, track);
            try
            {
                var trackItem = Tracks.Single(t => t.TrackId == track.TrackId);
                Tracks[Tracks.IndexOf(trackItem)].Popularity += 1;
            }
            catch
            {
                track.Popularity = 1;
                Tracks.Add(track);
            }
        }

        private void AddTrackToUserTracks(User user, Track track)
        {
            var userTracksList = GetUserTracks(user);
            
            if (userTracksList.Count >= 10)
            {
                return;
            }
            userTracksList.Add(track);
            UserTracks[user.UserId] = userTracksList;
        }

        private List<Track> GetUserTracks(User user)
        {
            var userTracks = new List<Track>();
            if (UserTracks.ContainsKey(user.UserId))
            {
                userTracks = UserTracks[user.UserId].ToList();
                return userTracks;
            }

            UserTracks.Add(user.UserId, new List<Track>());
            return userTracks;
        }

        public void SetOwner(User user)
        {
            Owner = user;
        }

        public void AddRedactor(User user)
        {
            Redactors.Add(user);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Playlist_for_party.Data;
using Playlist_for_party.Interfaсes.Services.Managers.DataManagers;
using WebApp_Data.Models;
using WebApp_Data.Models.DbConnections;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.TrackAbilities;
using WebApp_Data.Models.UserData;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Playlist_for_party.Services.Managers.DataManagers
{
    public class PlaylistDataManager : IPlaylistDataManager
    {
        private readonly MusicContext _musicContext;
        private readonly IDataManager _dataManager;

        public PlaylistDataManager(MusicContext musicContext,
            IDataManager dataManager)
        {
            _musicContext = musicContext;
            _dataManager = dataManager;
        }

        public void AddTrackToPlaylist(User user, Playlist playlist, Track track)
        {
            CreateNewPlaylistTrack(out var playlistDb, playlist, track);
            _musicContext.SaveChanges();
            AddTrackToUserTracks(user, playlistDb, track);
            _musicContext.SaveChanges();
            SetPopularityOnTrack(playlistDb, track);

            _musicContext.SaveChanges();
        }

        public IEnumerable<Track> GetTracksFromPlaylist(Playlist playlist)
        {
            return _musicContext.PlaylistTracks
                .Where(p => p.PlaylistId == playlist.Id)
                .Select(p => p.Track).OrderByDescending(p=>p.Rating).ToList();
        }

        public bool IsOwner(User user, Playlist playlist)
        {
            return playlist.Owner == user;
        }

        public bool IsRedactor(User user, Playlist playlist)
        {
            var playlistDb = _dataManager.GetUserEditorPlaylistByPlaylistId(playlist.Id);
            
            if (playlistDb == null)
            {
                return false;
            }
            
            return playlistDb.UserId == user.Id;
        }

        public IEnumerable<User> GetEditors(Playlist playlist)
        {
            var editors = _musicContext.UserEditorPlaylists
                .Where(p => p.PlaylistId == playlist.Id)!
                .Select(u => u.User).ToList();

            return editors;
        }

        public void SetRedactorToPlaylist(User user, Playlist playlist)
        {
            _musicContext.UserEditorPlaylists
                .Add(new UserEditorPlaylist()
            {
                User = user,
                Playlist = playlist
            });

            _musicContext.SaveChanges();
        }

        public int GetNumOfEditors(Playlist playlist)
        {
            return playlist.UserEditorPlaylists.Count;
        }

        public string GetResultOfAddingAbility(User user, Playlist playlist, Track track)
        {
            var key = user.Id;
            if (!playlist.UserTracks.ContainsKey(key))
            {
                return SerializeCheckTrackAbility(false, false);
            }

            var trackCount = playlist.UserTracks[key].Count();
            if (trackCount >= 10)
            {
                return SerializeCheckTrackAbility(true, false);
            }

            var existingTrack = playlist.UserTracks[key]
                .FirstOrDefault(ut => ut.Id == track.Id);
            return SerializeCheckTrackAbility(false, existingTrack != null);
        }

        public Dictionary<Guid, IEnumerable<Track>> GetUserTracksFromPlaylist(Playlist playlist)
        {
            var userTracksJson = _musicContext.Playlists
                .SingleOrDefault(p => p.Id == playlist.Id)!
                .UserTracksJson;

            if (userTracksJson == null)
            {
                return new Dictionary<Guid, IEnumerable<Track>>();
            }

            var userTracksForDis = JsonSerializer
                .Deserialize<Dictionary<string, IEnumerable<Track>>>(userTracksJson);
                
            var userTracksKeyGuid = new Dictionary<Guid, IEnumerable<Track>>();

            if (userTracksForDis == null)
            {
                return userTracksKeyGuid;
            }
            
            foreach (var userTrack in userTracksForDis)
            {
                userTracksKeyGuid.Add(Guid.Parse(userTrack.Key), userTrack.Value);
            }

            return userTracksKeyGuid;
        }
        

        private void AddTrackToUserTracks(User user, Playlist playlist, Track track)
        {
            var userTracksList = GetUserTracks(playlist, user);

            userTracksList.Add(track);
            _musicContext.SaveChanges();

            playlist.UserTracks[user.Id] = userTracksList;
            
            var userTracks = playlist.UserTracks;
            
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            var userTracksJson = JsonSerializer.Serialize(userTracks, options);

            _musicContext.Playlists.SingleOrDefault(p => p.Id == playlist.Id)!
                .UserTracksJson = userTracksJson ;
        }

        private List<Track> GetUserTracks(Playlist playlist, User user)
        {
            var userTracks = new List<Track>();

            playlist.UserTracks = GetUserTracksFromPlaylist(playlist);

            if (playlist.UserTracks != null && playlist.UserTracks.ContainsKey(user.Id))
            {
                userTracks = playlist.UserTracks[user.Id].ToList();
                return userTracks;
            }

            playlist.UserTracks?.Add(user.Id, new List<Track>());
            return userTracks;
        }

        private void CreateNewPlaylistTrack(out Playlist playlistDb, Playlist playlist, Track track)
        {
            playlistDb = _dataManager.GetPlaylistById(playlist.Id);

            if (_musicContext.PlaylistTracks.All(p => p.TrackId != track.Id))
            {
                _musicContext.PlaylistTracks
                    .Add(new PlaylistTrack()
                    {
                        Playlist = playlist,
                        Track = track
                    });
            }
        }

        private void SetPopularityOnTrack(Playlist playlist, Track track)
        {
            var trackDb = _dataManager.GetTrackById(track.Id);

            if (trackDb != null)
            {
                trackDb.Rating++;
            }
            _musicContext.SaveChanges();

            var playlistDb = _musicContext.Playlists
                .SingleOrDefault(p => p.Id == playlist.Id);
            
            if (playlistDb!.TracksRating != null)
            {
                playlist.TracksRating = JsonSerializer
                    .Deserialize<Dictionary<string, int>>(playlistDb!.TracksRatingJson);
            }
            else
            {
                playlist.TracksRating = new Dictionary<string, int>();
            }

            try
            {
                if (playlist.TracksRating != null)
                {
                    playlist.TracksRating[track.Id]++;
                }
            }
            catch
            {
                track.Rating = 1;

                if (_musicContext.Tracks.SingleOrDefault(t => t.Id == track.Id) == null)
                {
                    _musicContext.Tracks.Add(track);
                }

                playlist.TracksRating?.Add(track.Id, 1);
            }
            _musicContext.SaveChanges();
            playlistDb!.TracksRatingJson = JsonSerializer.Serialize(playlist.TracksRating);
            _musicContext.SaveChanges();
        }

        private static string SerializeCheckTrackAbility(bool exceedingTheLimit, bool trackDuplication)
        {
            return JsonSerializer.Serialize(new CheckTrackAbility()
            {
                ExceedingTheLimit = exceedingTheLimit,
                TrackDuplication = trackDuplication
            });
        }
    }
}
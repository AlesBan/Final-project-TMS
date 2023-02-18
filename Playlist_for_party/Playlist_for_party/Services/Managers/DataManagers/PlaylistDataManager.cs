using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Playlist_for_party.Data;
using Playlist_for_party.Interfaсes.Services.Managers.DataManagers;
using WebApp_Data.Models.DbConnections;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.TrackAbilities;
using WebApp_Data.Models.UserData;

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

        public void AddTrack(User user, Playlist playlist, Track track)
        {
            CreateNewPlaylistTrack(out var playlistDb, playlist, track);

            playlistDb?.AddTrackToUserTracks(user, track);

            GetTrackDbAndSetPopularity(playlistDb, track);

            _musicContext.SaveChanges();
        }

        public IEnumerable<Track> GetTracks(Playlist playlist)
        {
            return _musicContext.PlaylistTracks
                .Where(p => p.Equals(playlist))
                .Select(p => p.Track);
        }

        public bool IsOwner(User user, Playlist playlist)
        {
            return playlist.Owner == user;
        }

        public bool IsRedactor(User user, Playlist playlist)
        {
            var playlistDb = _dataManager.GetUserEditorPlaylistByPlaylistId(playlist.Id);
            
            return playlistDb.UserId == user.Id;
        }

        public void SetRedactorToPlaylist(User user, Playlist playlist)
        {
            var userDb = _dataManager.GetUserByUserName(user.UserName);

            userDb.UserEditorPlaylists.Add(new UserEditorPlaylist()
            {
                User = user,
                Playlist = playlist
            });

            _musicContext.SaveChangesAsync();
        }

        public int GetNumOfEditors(Playlist playlist)
        {
            return _musicContext.Playlists
                .SingleOrDefault(p => p.Equals(playlist))!
                .UserEditorPlaylists.Count;
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

        private void CreateNewPlaylistTrack(out Playlist playlistDb, Playlist playlist, Track track)
        {
            playlistDb = _dataManager.GetPlaylistById(playlist.Id);

            playlistDb?.PlaylistTracks
                .Add(new PlaylistTrack()
                {
                    Playlist = playlist,
                    Track = track
                });
        }

        private void GetTrackDbAndSetPopularity(Playlist playlist, Track track)
        {
            var trackDb = _dataManager.GetTrackById(track.Id);

            if (trackDb != null)
            {
                trackDb.Popularity++;
            }

            try
            {
                var trackInPlaylist = playlist.PlaylistTracks
                    .Single(t => t.TrackId == track.Id)
                    .Track;
                trackInPlaylist.Popularity++;
            }
            catch
            {
                track.Popularity = 1;
                _musicContext.Tracks.Add(track);
            }
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
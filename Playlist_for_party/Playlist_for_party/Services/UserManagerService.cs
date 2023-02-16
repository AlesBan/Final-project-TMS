using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Playlist_for_party.Exceptions.AppExceptions;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly IMusicDataManagerService _dataManager;
        private readonly IMusicService _musicService;
        
      
        public UserManagerService(IMusicDataManagerService dataManager, IMusicService musicService)
        {
            _dataManager = dataManager;
            _musicService = musicService;
        }

        public User GetCurrentUser(HttpContext context)
        {
            var userId = context.User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidClaimedUserIdException();
            }

            return _dataManager.GetUser(Guid.Parse(userId));
        }
        
        public void SetRedactor(User user, Playlist playlist)
        {
            playlist.AddRedactor(user);
            user.AddPlaylistAsRedactor(playlist);
        }

        public Playlist CreatePlaylist(User user)
        {
            var playlist = _dataManager.CreatePlaylist();
            playlist.SetOwner(user);
            user.AddPlaylistAsOwner(playlist);
            return playlist;
        }

        public void GetPlaylistAndTrack(HttpContext context, string trackId, string playlistId,
            out Playlist playlist, out Track track)
        {
            track = _musicService.GetTrack(trackId).Result;

            var playlistIdGuid = Guid.Parse(playlistId);

            playlist = _dataManager.GetPlaylists().FirstOrDefault(p => p.PlaylistId == playlistIdGuid);
        }

        public string GetResultOfAddingAbility(User user, Playlist playlist, Track track)
        {
            var key = user.UserId;
            if (!playlist.UserTracks.ContainsKey(key))
            {
                return SerializeCheckTrackAbility(false, false);
            }

            var trackCount = playlist.UserTracks[key].Count();
            if (trackCount >= 10)
            {
                return SerializeCheckTrackAbility(true, false);
            }
            
            var existingTrack = playlist.UserTracks[key].FirstOrDefault(ut => ut.TrackId == track.TrackId);
            return SerializeCheckTrackAbility(false, existingTrack != null);
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
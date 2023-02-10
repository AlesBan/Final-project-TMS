using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playlist_for_party.Exceptions.AppExceptions;
using Playlist_for_party.Interfaсes.Services;
using Unosquare.Swan.Formatters;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly IMusicDataManagerService _dataManager;
        private readonly IMusicService _musicService;
        private readonly IMusicDataManagerService _userManager;

        public UserManagerService(IMusicDataManagerService dataManager, 
            IMusicService musicService, IMusicDataManagerService userManager)
        {
            _dataManager = dataManager;
            _musicService = musicService;
            _userManager = userManager;
        }

        public void SetRedactor(User user, Playlist playlist)
        {
            playlist.AddRedactor(user);
            user.AddPlaylistAsRedactor(playlist);
        }

        public void CreatePlaylist(User user, out Playlist playlist)
        {
            playlist = _dataManager.CreatePlaylist();
            playlist.SetOwner(user);
            user.AddPlaylistAsOwner(playlist);
        }

        public void GetUserPlaylistAndTrack(HttpContext context, string trackId, string playlistId, 
            out User user, out Playlist playlist, out Track track)
        {
            user = GetCurrentUser(context);
            track = _musicService.GetTrack(trackId).Result;
            var playlistIdGuid = Guid.Parse(playlistId);
            var playlists = _dataManager.GetPlaylists();
            playlist = playlists.FirstOrDefault(p => p.PlaylistId.Equals(playlistIdGuid));
        }

        public string GetResultOfAddingAbility(Guid key, User user, Playlist playlist, Track track)
        {
            if (!(playlist is { UserTracks: { } }) || !playlist.UserTracks.ContainsKey(key) || track == null)
            {
                return Json.Serialize(new CheckTrackAbility(false, false));
            }

            if (playlist.UserTracks[key].Count() == 10)
            {
                return Json.Serialize(new CheckTrackAbility(true, false));
            }

            return playlist.UserTracks[key].Any(ut => ut.TrackId == track.TrackId)
                ? Json.Serialize(new CheckTrackAbility(false, true))
                : Json.Serialize(new CheckTrackAbility(false, false));
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
    }
}
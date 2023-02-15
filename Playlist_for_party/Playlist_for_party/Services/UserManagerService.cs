using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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

        public UserManagerService(IMusicDataManagerService dataManager, IMusicService musicService)
        {
            _dataManager = dataManager;
            _musicService = musicService;
        }

        public string CreateToken(User user, IConfiguration configuration)
        {
            var roles = new List<string>() { "user" };
            var token = Authentication.Authentication.GenerateToken(configuration, user, roles);
            return token;
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

            playlist = _dataManager.GetPlaylists().FirstOrDefault(p => p.PlaylistId == playlistIdGuid);
        }

        public string GetResultOfAddingAbility(Guid key, User user, Playlist playlist, Track track)
        {
            if (!(playlist is { UserTracks: { } }) || !playlist.UserTracks.ContainsKey(key) || track == null)
            {
                return "{\"ExceedingTheLimit\": false,\"TrackDuplication\": false}";
                return Json.Serialize(new CheckTrackAbility(false, false));
            }

            var count = playlist.UserTracks[key].Count();
            if (count == 10)
            {
                return "{\"ExceedingTheLimit\": true,\"TrackDuplication\": false}";
                return Json.Serialize(new CheckTrackAbility(true, false));
            }

            var hasTrack = playlist.UserTracks[key].Any(ut => ut.TrackId == track.TrackId);
            return hasTrack ? "{\"ExceedingTheLimit\": false,\"TrackDuplication\": true}" : "{\"ExceedingTheLimit\": false,\"TrackDuplication\": false}";
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
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Authentication.Controllers;
using WebApp_Data.Models;

namespace Playlist_for_party.Controllers
{
    public class TrackController: HomeController
    {
        private readonly IMusicService _musicService;

        public TrackController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTrackToPlaylist(string trackId, string playlistId)
        {
            var user = GetCurrentUser();
            var track = await _musicService.GetTrack(trackId);
            var playlistIdGuid = Guid.Parse(playlistId);
            var playlists = AccountController.MusicRepository.Playlists;
            var playlist = playlists.FirstOrDefault(p => p.PlaylistId.Equals(playlistIdGuid));

            AccountController.MusicRepository
                .Playlists[playlists.IndexOf(playlist)]
                .AddTrack(user, track);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> CheckTrackAbilityToBeAdded(string trackId, string playlistId)
        {
            var user = GetCurrentUser();
            var track = await _musicService.GetTrack(trackId);
            var playlistIdGuid = Guid.Parse(playlistId);
            var playlists = AccountController.MusicRepository.Playlists;
            var playlist = playlists.FirstOrDefault(p => p.PlaylistId.Equals(playlistIdGuid));

            var key = Guid.Parse($"{user.UserId}");
            if (!(playlist is { UserTracks: { } }) || !playlist.UserTracks.ContainsKey(key) || track == null)
            {
                return Json(new CheckTrackAbility(false, false));
            }

            if (playlist.UserTracks[key].Count() == 10)
            {
                return Json(new CheckTrackAbility(true, false));
            }

            return playlist.UserTracks[key].Any(ut => ut.TrackId == track.TrackId)
                ? Json(new CheckTrackAbility(false, true))
                : Json(new CheckTrackAbility(false, false));
        }
    }
}
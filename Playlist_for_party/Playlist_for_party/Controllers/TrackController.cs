using System;
using Microsoft.AspNetCore.Mvc;
using Playlist_for_party.Interfaсes.Services;

namespace Playlist_for_party.Controllers
{
    public class TrackController : Controller
    {
        private readonly IMusicDataManagerService _dataManager;
        private readonly IUserManagerService _userManager;

        public TrackController(IMusicDataManagerService dataManager, IUserManagerService userManager)
        {
            _dataManager = dataManager;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult AddTrackToPlaylist(string trackId, string playlistId)
        {
            _userManager.GetUserPlaylistAndTrack(HttpContext, trackId, playlistId,
                out var user, out var playlist, out var track);

            _dataManager.AddTrack(user, playlist, track);

            return NoContent();
        }

        [HttpGet]
        public ActionResult<string> CheckTrackAbilityToBeAdded(string trackId, string playlistId)
        {
            _userManager.GetUserPlaylistAndTrack(HttpContext, trackId, playlistId,
                out var user, out var playlist, out var track);

            var key = Guid.Parse($"{user.UserId}");

            return _userManager.GetResultOfAddingAbility(key, user, playlist, track);
        }
    }
}
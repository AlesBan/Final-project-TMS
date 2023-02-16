using System;
using Microsoft.AspNetCore.Mvc;
using Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions;
using Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions.NotFoundExceptions;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;

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
            ValidateIncomeData_AddTrackToPlaylist(trackId, playlistId);

            var user = _userManager.GetCurrentUser(HttpContext);
            _userManager.GetPlaylistAndTrack(HttpContext, trackId, playlistId, out var playlist, out var track);

            ValidateMusicData_AddTrackToPlaylist(user, playlist, track);

            _dataManager.AddTrack(user, playlist, track);

            return NoContent();
        }

        [HttpGet]
        public ActionResult<string> CheckTrackAbilityToBeAdded(string trackId, string playlistId)
        {
            var user = _userManager.GetCurrentUser(HttpContext);

            _userManager.GetPlaylistAndTrack(HttpContext, trackId, playlistId, out var playlist, out var track);

            return _userManager.GetResultOfAddingAbility(user, playlist, track);
        }
        
        private static void ValidateMusicData_AddTrackToPlaylist(User user, Playlist playlist, Track track)
        {
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (playlist == null)
            {
                throw new PlaylistNotFoundException();
            }

            if (track == null)
            {
                throw new TrackNotFoundException();
            }
        }

        private void ValidateIncomeData_AddTrackToPlaylist(string trackId, string playlistId)
        {
            if (string.IsNullOrEmpty(trackId))
            {
                throw new InvalidTrackIdProvidedException("Track id not found");
            }

            if (string.IsNullOrEmpty(playlistId))
            {
                throw new InvalidPlaylistIdProvidedException("Playlist id not found");
            }

            if (!Guid.TryParse(playlistId, out _))
            {
                throw new InvalidPlaylistIdProvidedException("Playlist id not invalid");
            }
        }

    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions;
using Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions.NotFoundExceptions;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Interfaсes.Services.Managers.DataManagers;
using Playlist_for_party.Interfaсes.Services.Managers.UserManagers;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.UserData;

namespace Playlist_for_party.Controllers
{
    public class TrackController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IPlaylistDataManager _playlistDataManager;
        private readonly IMusicService _musicService;
        private readonly IDataManager _dataManager;

        public TrackController(IUserManager userManager, IPlaylistDataManager playlistDataManager, 
            IMusicService musicService, IDataManager dataManager)
        {
            _userManager = userManager;
            _playlistDataManager = playlistDataManager;
            _musicService = musicService;
            _dataManager = dataManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddTrackToPlaylist(string trackId, string playlistId)
        {
            ValidateIncomeData_AddTrackToPlaylist(trackId, playlistId);

            var user = _userManager.GetCurrentUser(HttpContext);
            var track = await _musicService.GetTrackFromSpotifyApi(trackId);
            var playlist = _dataManager.GetPlaylistById(Guid.Parse(playlistId));

            ValidateMusicData_AddTrackToPlaylist(user, playlist, track);

            _playlistDataManager.AddTrack(user, playlist, track);

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<string>> CheckTrackAbilityToBeAdded(string trackId, string playlistId)
        {
            var user = _userManager.GetCurrentUser(HttpContext);

            var track = await _musicService.GetTrackFromSpotifyApi(trackId);
            var playlist = _dataManager.GetPlaylistById(Guid.Parse(playlistId));

            return _playlistDataManager.GetResultOfAddingAbility(user, playlist, track);
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
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Playlist_for_party.Exceptions.AppExceptions;
using Playlist_for_party.Filters.ExceptionFilters;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMusicService _musicService;
        private readonly IMusicDataManagerService _dataManager;

        public HomeController(IMusicService spotifyService, IMusicDataManagerService dataManager)
        {
            _musicService = spotifyService;
            _dataManager = dataManager;
        }
        
        [Route("home")]
        public IActionResult Home()
        {
            var user = GetCurrentUser();

            if (user is null)
            {
                return Unauthorized();
            }

            ViewBag.PlaylistsAsOwner = user.PlaylistsAsOwner;
            ViewBag.PlaylistsAsRedactor = user.PlaylistsAsRedactor;
            return View();
        }

        [ExceptionFilter]
        [HttpGet("search/{query?}")]
        public IActionResult Search(string query)
        {
            var user = GetCurrentUser();

            if (user is null)
            {
                return Unauthorized();
            }
            
            if (string.IsNullOrEmpty(query))
            {
                return View();
            }

            var searchItems = _musicService.GetItems(query).Result;
            ViewBag.query = query;
            ViewBag.Playlists =_dataManager.GetPlaylists();
            ViewBag.Artists = searchItems.ArtistDtos;
            ViewBag.Tracks = searchItems.TrackDtos;
            return View();
        }

        [ExceptionFilter]
        [HttpGet("playlist/{playlistId:guid?}")]
        public IActionResult Playlist(Guid playlistId)
        {
            Playlist playlist;
            var user = GetCurrentUser();

            if (user is null)
            {
                return Unauthorized();
            }

            if (playlistId == Guid.Empty)
            {
                playlist = _dataManager.CreatePlaylist();
                playlist.SetOwner(user);
                user.AddPlaylistAsOwner(playlist);
                return Redirect($"playlist/{playlist.PlaylistId}");
            }

            playlist =_dataManager.GetPlaylist(playlistId);

            if (user.IsOwner(playlist) || user.IsRedactor(playlist))
            {
                return View(playlist);
            }

            playlist.AddRedactor(user);
            user.AddPlaylistAsRedactor(playlist);

            return View(playlist);
        }

        [HttpPost]
        public async Task<IActionResult> AddTrackToPlaylist(string trackId, string playlistId)
        {
            var user = GetCurrentUser();
            var track = await _musicService.GetTrack(trackId);
            var playlistIdGuid = Guid.Parse(playlistId);
            var playlists = _dataManager.GetPlaylists();
            var playlist = playlists.FirstOrDefault(p => p.PlaylistId.Equals(playlistIdGuid));
            _dataManager.AddTrack(user, playlist, track);
            
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> CheckTrackAbilityToBeAdded(string trackId, string playlistId)
        {
            var user = GetCurrentUser();
            var track = await _musicService.GetTrack(trackId);
            var playlistIdGuid = Guid.Parse(playlistId);
            var playlists = _dataManager.GetPlaylists();
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

        private User GetCurrentUser()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidClaimedUserIdException();
            }
            return _dataManager.GetUser(Guid.Parse(userId));
        }

        [Route("forbidden")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
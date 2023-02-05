using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Playlist_for_party.Filters.ExceptionFilters;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Authentication.Controllers;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMusicService _musicService;

        public HomeController(IMusicService spotifyService)
        {
            _musicService = spotifyService;
        }

        [HttpGet("home")]
        public IActionResult Home()
        {
            var user = GetCurrentUser();
            ViewBag.PlaylistsAsOwner = user.PlaylistsAsOwner;
            ViewBag.PlaylistsAsRedactor = user.PlaylistsAsRedactor;
            return View();
        }

        [ExceptionFilter]
        [HttpGet("search/{query?}")]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View();
            }

            var searchItems = _musicService.GetItems(query).Result;
            ViewBag.query = query;
            ViewBag.Playlists = AccountController.MusicRepository.Playlists;
            ViewBag.Artists = searchItems.ArtistDtos;
            ViewBag.Tracks = searchItems.TrackDtos;
            return View();
        }

        [ExceptionFilter]
        [HttpGet("playlist/{id:guid?}")]
        public IActionResult Playlist(Guid id)
        {
            Playlist playlist;
            var userId = Guid.Parse(HttpContext.Request.Cookies[$"UserId"]);
            var user = AccountController.MusicRepository.GetUser(userId);

            if (user is null)
            {
                return Unauthorized();
            }

            if (id == Guid.Empty)
            {
                playlist = AccountController.MusicRepository.CreatePlaylist();
                playlist.SetOwner(user);
                user.AddPlaylistAsOwner(playlist);
                return Redirect($"playlist/{playlist.PlaylistId}");
            }

            playlist = AccountController.MusicRepository.GetPlaylist(id);

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
            if (playlist is { UserTracks: { } } && playlist.UserTracks.ContainsKey(key) && track != null)
            {
                if (playlist.UserTracks[key].Count() == 10)
                {
                    return Json(new CheckTrackAbility(true, false));
                }

                return playlist.UserTracks[key].Any(ut => ut.TrackId == track.TrackId)
                    ? Json(new CheckTrackAbility(false, true))
                    : Json(new CheckTrackAbility(false, false));
            }

            return Json(new CheckTrackAbility(false, false));
        }

        private User GetCurrentUser()
        {
            var userId = Guid.Parse(HttpContext.Request.Cookies["UserId"]);
            return AccountController.MusicRepository.GetUser(userId);
        }

        [Route("forbidden")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
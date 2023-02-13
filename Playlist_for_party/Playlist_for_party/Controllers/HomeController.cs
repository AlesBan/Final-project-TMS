using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions.NotFoundExceptions;
using Playlist_for_party.Filters.ExceptionFilters;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMusicService _musicService;
        private readonly IMusicDataManagerService _dataManager;
        private readonly IUserManagerService _userManager;

        public HomeController(IMusicService spotifyService, IMusicDataManagerService dataManager,
            IUserManagerService userManager)
        {
            _musicService = spotifyService;
            _dataManager = dataManager;
            _userManager = userManager;
        }

        [Route("home")]
        public IActionResult Home()
        {
            var user = _userManager.GetCurrentUser(HttpContext);

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
            var user = _userManager.GetCurrentUser(HttpContext);

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
            ViewBag.Playlists = _dataManager.GetPlaylists();
            ViewBag.Artists = searchItems.ArtistDtos;
            ViewBag.Tracks = searchItems.TrackDtos;
            return View();
        }

        [ExceptionFilter]
        [HttpGet("playlist/{id:guid?}")]
        public IActionResult Playlist(Guid id)
        {
            var user = _userManager.GetCurrentUser(HttpContext);

            if (user is null)
            {
                return Unauthorized();
            }

            Playlist playlist;
            if (id == Guid.Empty)
            {
                _userManager.CreatePlaylist(user, out playlist);
                return Redirect($"playlist/{playlist.PlaylistId}");
            }

            playlist = _dataManager.GetPlaylist(id);

            if (playlist is null)
            {
                throw new PlaylistNotFoundException();
            }

            if (user.IsOwner(playlist) || user.IsRedactor(playlist))
            {
                return View(playlist);
            }

            _userManager.SetRedactor(user, playlist);

            return View(playlist);
        }

        [Route("forbidden")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
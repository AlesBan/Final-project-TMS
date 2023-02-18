using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions.NotFoundExceptions;
using Playlist_for_party.Filters.ExceptionFilters;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Interfaсes.Services.Managers.DataManagers;
using Playlist_for_party.Interfaсes.Services.Managers.UserManagers;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMusicService _musicService;
        private readonly IDataManager _dataManager;
        private readonly IUserManager _userManager;
        private readonly IPlaylistDataManager _playlistDataManager;

        public HomeController(IMusicService spotifyService, IDataManager dataManager,
            IUserManager userManager, IPlaylistDataManager playlistDataManager)
        {
            _musicService = spotifyService;
            _dataManager = dataManager;
            _userManager = userManager;
            _playlistDataManager = playlistDataManager;
        }

        [Route("home")]
        public IActionResult Home()
        {
            var user = _userManager.GetCurrentUser(HttpContext);

            if (user is null)
            {
                return Unauthorized();
            }

            ViewBag.PlaylistsAsOwner = _dataManager.GetPlaylistsWhereUserOwner(user);
            ViewBag.PlaylistsAsRedactor = _dataManager.GetPlaylistsWhereUserEditor(user);
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

            var searchItems = _musicService.GetItemsFromSpotifyApi(query).Result;
            ViewBag.query = query;
            ViewBag.Artists = searchItems.ArtistsDto;
            ViewBag.Tracks = searchItems.TracksDto;
            ViewBag.Playlists = _dataManager.GetPlaylistsWhereUserEditor(user);
            
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
                playlist = _dataManager.CreatePlaylist(user);
                return Redirect($"playlist/{playlist.Id}");
            }

            playlist = _dataManager.GetPlaylistById(id);
            ViewBag.Tracks = _playlistDataManager.GetTracksFromPlaylist(playlist);

            if (playlist is null)
            {
                throw new PlaylistNotFoundException();
            }

            if (_playlistDataManager.IsOwner(user, playlist) || _playlistDataManager.IsRedactor(user, playlist))
            {
                return View();
            }
            
            _playlistDataManager.SetRedactorToPlaylist(user, playlist);
            
            return View();
        }

        [Route("forbidden")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
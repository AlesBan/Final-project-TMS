using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playlist_for_party.Filters.ExceptionFilters;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Authentication.Controllers;
using WebApp_Authentication.Models;
using WebApp_Data.Models.Music;

namespace Playlist_for_party.Controllers
{
    public class UserController : HomeController
    {
        private readonly IMusicService _musicService;

        public UserController(IMusicService spotifyService)
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
            var user = GetCurrentUser();

            if (user is null)
            {
                return Unauthorized();
            }

            Playlist playlist;
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

        [Route("forbidden")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
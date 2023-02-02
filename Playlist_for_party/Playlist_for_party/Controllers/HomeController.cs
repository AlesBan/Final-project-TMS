using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Playlist_for_party.Filters.ExceptionFilters;
using Playlist_for_party.Interfaсes.Services;

namespace Playlist_for_party.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMusicService _musicService;

        public HomeController(IMusicService spotifyService)
        {
            _musicService = spotifyService;
        }

        [Authorize]
        [HttpGet("home")]
        public IActionResult Home()
        {
            return View(Startup.MusicRepository);
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
            ViewBag.Playlists = Startup.MusicRepository.Playlists;
            ViewBag.Artists = searchItems.ArtistDtos;
            ViewBag.Tracks = searchItems.TrackDtos;
            return View();
        }

        [ExceptionFilter]
        [HttpGet("playlist/{id:guid?}")]
        public IActionResult Playlist(Guid id)
        {
            if (id != Guid.Empty)
            {
                return View(Startup.MusicRepository.GetPlaylist(id));
            }

            var playlist = Startup.MusicRepository.CreatePlaylist();
            return Redirect($"playlist/{playlist.PlaylistId}");
        }

        [ExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> AddTrackToPlaylist(string trackId, string playlistId)
        {
            var track = await _musicService.GetTrack(trackId);
            var playlistIdGuid = Guid.Parse(playlistId);
            var playlists = Startup.MusicRepository.Playlists;
            Startup.MusicRepository
                .Playlists[playlists.IndexOf(playlists.FirstOrDefault(p => p.PlaylistId.Equals(playlistIdGuid)))]
                .AddTrack(track);
            return NoContent();
        }

        [Route("forbidden")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
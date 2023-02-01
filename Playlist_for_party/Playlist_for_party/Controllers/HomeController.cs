using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Playlist_for_party.Attributes;
using Playlist_for_party.Interfaсes;
using Playlist_for_party.Interfaсes.Services;

namespace Playlist_for_party.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpotifyService _spotifyService;
        private IMusicRepository MusicRepository { get; }


        public HomeController(ISpotifyService spotifyService, IMusicRepository musicRepository)
        {
            _spotifyService = spotifyService;
            MusicRepository = musicRepository;
        }

        [HttpGet("home")]
        public ViewResult Home()
        {
            
            return View(MusicRepository);
        }

        [ExceptionFilter]
        [HttpGet("search/{query?}")]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View();
            }

            var searchItems = _spotifyService.GetItems(query).Result;
            ViewBag.query = query;
            ViewBag.Playlists = MusicRepository.Playlists;
            ViewBag.Artists = searchItems.ArtistDtos;
            ViewBag.Tracks = searchItems.TrackDtos;
            return View();
        }

        [HttpGet("playlist/{id:guid?}")]
        public IActionResult Playlist(Guid id)
        {
            if (id != Guid.Empty)
            {
                return View(MusicRepository.GetPlaylist(id));
            }

            var playlist = MusicRepository.CreatePlaylist();
            return Redirect($"playlist/{playlist.PlaylistId}");
        }

        [ExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> AddTrackToPlaylist(string trackId, string playlistId)
        {
            var track = await _spotifyService.GetTrack(trackId);
            var playlistIdGuid = Guid.Parse(playlistId);
            var playlists = MusicRepository.Playlists;
            MusicRepository.Playlists[playlists.IndexOf(playlists.First(p => p.PlaylistId.Equals(playlistIdGuid)))]
                .AddTrack(track);
            return NoContent();
        }
    }
}
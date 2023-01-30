using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Playlist_for_party.Data;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Models.Music;
using Playlist_for_party.Models.SpotifyModels.DTO;

namespace Playlist_for_party.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly ISpotifyService _spotifyService;
        private const int LimitNum = 5;
        private const string CountryCode = "BY";
        private IMusicRepository MusicRepository { get; }


        public HomeController(ISpotifyAccountService spotifyAccountService, ISpotifyService spotifyService,
            IMusicRepository musicRepository)
        {
            _spotifyAccountService = spotifyAccountService;
            _spotifyService = spotifyService;
            MusicRepository = musicRepository;
        }

        [HttpGet("home")]
        public ViewResult Home()
        {
            return View(MusicRepository);
        }

        [HttpGet("search/{query?}")]
        public ViewResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(Enumerable.Empty<ItemDto>());
            }
            else
            {
                var searchItems = GetItems(query).Result;
                ViewBag.query = query;
                ViewBag.Playlists = MusicRepository.Playlists;
                return View(searchItems);
            }
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

        [HttpPost]
        public async Task<IActionResult> AddTrackToPlaylist(string trackId, string playlistId)
        {
            try
            {
                var random = new Random();
                var token = await _spotifyAccountService.GetAccessToken();

                var track = await _spotifyService.GetTrack(token, trackId);
                var playlists = MusicRepository.Playlists;
                var playlistIdGuid = Guid.Parse(playlistId);
                MusicRepository.Playlists[playlists.IndexOf(playlists.First(p => p.PlaylistId.Equals(playlistIdGuid)))]
                    .AddTrack(track);
                return NoContent();
            }
            catch (Exception ex)
            {
                Debug.Write(ex);

                return Ok();
            }
        }

        private async Task<IEnumerable<ReleaseDto>> GetReleases()
        {
            try
            {
                var token = await _spotifyAccountService.GetAccessToken();

                var newReleases = await _spotifyService.GetNewReleases(CountryCode, LimitNum, token);
                return newReleases;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);

                return Enumerable.Empty<ReleaseDto>();
            }
        }

        private async Task<IEnumerable<ItemDto>> GetItems(string query)
        {
            try
            {
                var token = await _spotifyAccountService.GetAccessToken();
                var items = await _spotifyService.GetItems(LimitNum, token, query);
                return items;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);

                return Enumerable.Empty<ItemDto>();
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Playlist_for_party.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Models.Music;
using Playlist_for_party.Models.SpotifyApiConnection;

namespace Playlist_for_party.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly IConfiguration _configuration;
        private readonly ISpotifyService _spotifyService;

        public HomeController(
            ISpotifyAccountService spotifyAccountService,
            IConfiguration configuration,
            ISpotifyService spotifyService)
        {
            _spotifyAccountService = spotifyAccountService;
            _configuration = configuration;
            _spotifyService = spotifyService;
        }
        
        [Route("homepage")]
        public async Task<IActionResult> HomePage()
        {
            var newReleases = await GetReleases();

            return View(newReleases);
        }
        
        [Route("artists/{query}")]
        public async Task<IActionResult> Artists(string query)
        {
            var artists = await GetArtists(query);

            return View(artists);
        }

        private async Task<IEnumerable<Release>> GetReleases()
        {
            try
            {
                var token = await _spotifyAccountService.GetToken(
                    _configuration["Spotify:ClientId"],
                    _configuration["Spotify:ClientSecret"]);

                var newReleases = await _spotifyService.GetNewReleases("BY", 20, token);
                return newReleases;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);

                return Enumerable.Empty<Release>();
            }
        }
        
        private async Task<IEnumerable<ArtistMy>> GetArtists(string query)
        {
            try
            {
                var token = await _spotifyAccountService.GetToken(
                    _configuration["Spotify:ClientId"],
                    _configuration["Spotify:ClientSecret"]);
                var artists = await _spotifyService.GetItems(20, token, query);
                return artists;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);

                return Enumerable.Empty<Playlist_for_party.Models.Music.ArtistMy>();
            }
        }
    }
}

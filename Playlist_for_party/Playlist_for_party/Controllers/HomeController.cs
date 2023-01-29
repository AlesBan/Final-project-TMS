﻿using Microsoft.AspNetCore.Mvc;
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
        
        public HomeController(ISpotifyAccountService spotifyAccountService, ISpotifyService spotifyService)
        {
            _spotifyAccountService = spotifyAccountService;
            _spotifyService = spotifyService;
        }
        
        [Route("home")]
        public async Task<IActionResult> Home()
        {
            var newReleases = await GetReleases();

            return View(newReleases);
        }
        [Route("search/{query?}")]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(Enumerable.Empty<ItemDto>());
            }
            else
            {
                var searchItems = await GetItems(query);
                ViewBag.query = query;
                return View(searchItems);
            }
        }
        
        [Route("playlist")]
        public IActionResult Playlist()
        {
            return View(Startup.MusicRepository.Playlist);
        }
        
        
        public async Task<IActionResult> AddTrackToPlaylist(string trackId)
        {
            try
            {
                var token = await _spotifyAccountService.GetAccessToken();

                var track = await _spotifyService.GetTrack(token, trackId);
                Startup.MusicRepository.Playlist.AddTrack(track);
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

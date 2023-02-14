using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Playlist_for_party.Exceptions.AppExceptions;
using Playlist_for_party.Exceptions.UserExceptions;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.SpotifyModels.DTO;
using WebApp_Data.Models.SpotifyModels.Main;
using WebApp_Data.Models.SpotifyModels.Secondary;

namespace Playlist_for_party.Services
{
    public class SpotifyService : IMusicService
    {
        private readonly HttpClient _httpClient;
        private readonly ISpotifyAccountService _spotifyAccountService;
        private const int Limit = 6;

        private const string DefImageUrl =
            @"https://media.wired.com/photos/5f9ca518227dbb78ec30dacf/master/w_2560%2Cc_limit/Gear-RIP-Google-Music-1194411695.jpg";

        public SpotifyService(HttpClient httpClient, ISpotifyAccountService spotifyAccountService)
        {
            _httpClient = httpClient;
            _spotifyAccountService = spotifyAccountService;
        }

        public async Task<Track> GetTrack(string trackId)
        {
            var accessToken = await _spotifyAccountService.GetAccessToken();
            Authorization(accessToken);
            var requestMessage = CreateRequest($"tracks/{trackId}");
            var response = await GetResponse(requestMessage);
            var responseObj = await DeserializationAsync<Item>(response);
            var firstImage = responseObj.Album.Images?[0];
            var track = new Track
            {
                TrackId = responseObj.Id,
                Name = responseObj.Name,
                ArtistName = string.Join(", ", responseObj.Artists.Select(a => a.Name)),
                Album = responseObj.Album.Name,
                Duration = responseObj.DurationMs,
                ImageUrl = firstImage?.Url ?? DefImageUrl,
                Href = responseObj.Href
            };
            return track;
        }

        public async Task<ItemsDto> GetItems(string query)
        {
            var accessToken = await _spotifyAccountService.GetAccessToken();
            Authorization(accessToken);
            var requestMessage = CreateRequest(query, true, true);
            var response = await GetResponse(requestMessage);
            var responseObject = await DeserializationAsync<Search>(response);
            return GetItemsDtoList(responseObject);
        }

        private static ItemsDto GetItemsDtoList(Search responseObj)
        {
            var artistsDto = responseObj?.Artists.Items.Where(i => i.Images?.Length > 0)
                .Select(i => new ArtistDto()
                {
                    Name = i.Name,
                    ImageRef = i.Images[0].Url,
                    Href = i.Href
                });

            var tracksDto = responseObj?.Tracks.Items.Where(i => i.Album.Images != null)
                .Select(i => new TrackDto()
                {
                    Name = i.Name,
                    ImageRef = i.Album.Images[0].Url,
                    Href = i.Href,
                    Id = i.Id,
                    ArtistName = string.Join(", ", i.Artists.Select(a => a.Name))
                });

            var itemsDto = new ItemsDto()
            {
                ArtistsDto = artistsDto?.ToList() ?? new List<ArtistDto>(),
                TracksDto = tracksDto?.ToList() ?? new List<TrackDto>()
            };

            return itemsDto;
        }

        private void Authorization(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private async Task<HttpResponseMessage> GetResponse(HttpRequestMessage requestMessage)
        {
            var response = await _httpClient.GetAsync(requestMessage.RequestUri);
            CheckResponse(response);
            return response;
        }

        private static HttpRequestMessage CreateRequest(string query)
        {
            return new HttpRequestMessage()
            {
                RequestUri = new Uri(query)
            };
        }

        private static HttpRequestMessage CreateRequest(string query, bool needArtists, bool needTracks)
        {
            var decodedQuery = HttpUtility.UrlEncode(query);
            var requestType = needTracks && needArtists ? "track" + "%2C" + "artist" : needArtists ? "artist" : "track";
            var requestUri = $"search?q={decodedQuery}&type={requestType}&market=BY&limit={Limit}";
            return new HttpRequestMessage()
            {
                RequestUri = new Uri(requestUri)
            };
        }

        private static void CheckResponse(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new BadRequestToSpotifyApiException();
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException();
            }
        }

        private static async Task<T> DeserializationAsync<T>(HttpResponseMessage response)
        {
            T responseObj;
            try
            {
                responseObj = await response.Content.ReadFromJsonAsync<T>();
            }
            catch (JsonException ex)
            {
                throw new DeserializationOfSpotifyModelException(ex.Message);
            }

            return responseObj;
        }
    }
}